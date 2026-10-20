namespace WebAPI.Services
{
    using Business.Abstract;
    using DataAccess.Abstract;
    using Entities.Concrete;
    using global::WebAPI.Hubs;
    using global::WebAPI.Hubs.WebAPI.Hubs;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using S7.Net;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    namespace WebAPI.Services
    {
        public class PlcPollingBackgroundService : BackgroundService
        {
            private readonly IS7DriverDal _s7DriverDal;
            private readonly IHubContext<ScadaHub> _scadaHubContext;
            private readonly IHubContext<AlarmHub> _alarmHubContext;
            private readonly IServiceScopeFactory _scopeFactory;
            private const string PlcIp = "192.168.0.1";

            public PlcPollingBackgroundService(
                IS7DriverDal s7DriverDal,
                IHubContext<ScadaHub> scadaHubContext,
                IHubContext<AlarmHub> alarmHubContext,
                IServiceScopeFactory scopeFactory)
            {
                _s7DriverDal = s7DriverDal;
                _scadaHubContext = scadaHubContext;
                _alarmHubContext = alarmHubContext;
                _scopeFactory = scopeFactory;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                // 1. İlk PLC Bağlantı Denemesi
                await _s7DriverDal.ConnectAsync(PlcIp, CpuType.S71200, 0, 1);

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_s7DriverDal.IsConnected)
                    {
                        try
                        {
                            // 2. PLC DB1 (DB_PLC_To_HMI) Bloğunu Tek Pakette Okuma (Bulk Read)
                            var furnaceData = await _s7DriverDal.ReadClassAsync(new FurnaceDbModel(), dbNumber: 1, startByte: 0);

                            if (furnaceData != null)
                            {
                                // A) SignalR Üzerinden Canlı Telemetri Yayınlama (ScadaHub)
                                await _scadaHubContext.Clients.All.SendAsync("ReceiveScadaData", new
                                {
                                    Timestamp = DateTime.Now,
                                    Zone1Temperature = furnaceData.Zone1Temperature,
                                    Zone2Temperature = furnaceData.Zone2Temperature,
                                    ChamberPressure = furnaceData.ChamberPressure,
                                    GasFlowRate = furnaceData.GasFlowRate,
                                    StatusWord = furnaceData.StatusWord,
                                    AlarmWord = furnaceData.AlarmWord,
                                    IsConnected = true
                                }, stoppingToken);

                                // B) Scoped Servisleri Tetikleme (Historian ve Alarm İşleme)
                                using var scope = _scopeFactory.CreateScope();
                                var historianService = scope.ServiceProvider.GetRequiredService<IHistorianService>();
                                var alarmService = scope.ServiceProvider.GetRequiredService<IAlarmService>();

                                // Historian Kaydı (Veri Tabanına Loglama)
                                await historianService.LogProcessDataAsync(new ProcessDataLog
                                {
                                    Timestamp = DateTime.Now,
                                    Zone1Temperature = furnaceData.Zone1Temperature,
                                    Zone2Temperature = furnaceData.Zone2Temperature,
                                    ChamberPressure = furnaceData.ChamberPressure,
                                    GasFlowRate = furnaceData.GasFlowRate,
                                    BurnerStatus = (furnaceData.StatusWord & 0x01) != 0
                                });

                                // Alarm Bit Haritasının Çözümlenmesi ve Yayınlanması (AlarmHub)
                                var alarmBits = ParseAlarmWord(furnaceData.AlarmWord);
                                var alarmResult = await alarmService.ProcessAlarmBitsAsync(alarmBits);
                                // Success yerine IsSuccess kullanıyoruz
                                if (alarmResult.IsSuccess)
                                {
                                    var activeAlarms = await alarmService.GetActiveAlarmsAsync();

                                    if (activeAlarms.IsSuccess && activeAlarms.Data != null)
                                    {
                                        await _alarmHubContext.Clients.All.SendAsync("ReceiveActiveAlarms", activeAlarms.Data, stoppingToken);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            await _scadaHubContext.Clients.All.SendAsync("ReceiveScadaError", ex.Message, stoppingToken);
                        }
                    }
                    else
                    {
                        // Bağlantı Koptuysa Yeniden Bağlanmayı Dene
                        await _s7DriverDal.ConnectAsync(PlcIp, CpuType.S71200, 0, 1);
                    }

                    // 500 ms Periyotla Polling
                    await Task.Delay(500, stoppingToken);
                }
            }

            // PLC AlarmWord (16-bit) İçerisindeki Bitleri Ayrıştırır
            private Dictionary<string, bool> ParseAlarmWord(ushort alarmWord)
            {
                return new Dictionary<string, bool>
            {
                { "ERR_ZONE1_OVERTEMP", (alarmWord & (1 << 0)) != 0 },
                { "ERR_ZONE2_OVERTEMP", (alarmWord & (1 << 1)) != 0 },
                { "ERR_PRESSURE_LOW",   (alarmWord & (1 << 2)) != 0 },
                { "ERR_BURNER_FAULT",   (alarmWord & (1 << 3)) != 0 }
            };
            }
        }
    }
}

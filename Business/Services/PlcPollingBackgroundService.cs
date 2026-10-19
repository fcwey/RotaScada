using Business.Abstract;
using DataAccess.Abstract;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using S7.Net;
using YourProjectNamespace.Hubs; // ScadaHub burada tanımlıysa burayı değiştirin

namespace Business.Services
{
    public class PlcPollingBackgroundService : BackgroundService
    {
        private readonly IS7DriverDal _s7DriverDal;
        private readonly IHubContext<ScadaHub> _hubContext;

        public PlcPollingBackgroundService(IS7DriverDal s7DriverDal, IHubContext<ScadaHub> hubContext)
        {
            _s7DriverDal = s7DriverDal;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // PLC IP Adresi
            _s7DriverDal.Connect("192.168.0.1", CpuType.S71200, 0, 1);

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_s7DriverDal.IsConnected)
                {
                    try
                    {
                        // DB1.CBD0 (Real -> Sıcaklık) okuma
                        var tempRaw = await _s7DriverDal.ReadTagAsync(DataType.DataBlock, 1, 0, VarType.Real);
                        float currentTemp = tempRaw != null ? Convert.ToSingle(tempRaw) : 0f;

                        // SignalR üzerinden istemcilere canlı yayın
                        await _hubContext.Clients.All.SendAsync("ReceiveScadaData", new
                        {
                            Timestamp = DateTime.Now,
                            Temperature = currentTemp,
                            IsConnected = true
                        }, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveScadaError", ex.Message, stoppingToken);
                    }
                }
                else
                {
                    // Bağlantı koptuysa yeniden bağlanmayı dene
                    _s7DriverDal.Connect("192.168.0.1", CpuType.S71200, 0, 1);
                }

                // 500 ms periyotla polling
                await Task.Delay(500, stoppingToken);
            }
        }
    }
}

namespace YourProjectNamespace.Hubs
{
    public class ScadaHub : Hub
    {
        // hub metodları
    }
}
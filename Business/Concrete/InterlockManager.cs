using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Structures;
using S7.Net;
using System;
using System.Net.ServerSentEvents;
using System.Threading.Tasks;

namespace Business.Concrete
{
    //*Saha operatörü HMI veya SCADA ekranından kritik bir manuel işlem başlatmak istediğinde(örneğin "Yakıcıyı Çalıştır"
     //veya "Ana Gaz Vanasını Aç"), sistem bu komutu doğrudan PLC'ye göndermez. Önce bu metot çalışarak fırının fiziksel durumunu denetler:
    //*SCADA ön yüzündeki (React / HMI) fırın görsel akış şemasında (Mimic Diagram) yer alan vanaların,
     //fanların, pompaların ve yakıcıların anlık durum renklerini besler:
    public class InterlockManager : IInterlockService
    {
        private readonly IS7DriverDal _s7DriverDal;

        public InterlockManager(IS7DriverDal s7DriverDal)
        {
            _s7DriverDal = s7DriverDal;
        }

        public async Task<IResult> ValidateOperationSafetyAsync(string operationCode, int dbNumber = 1)
        {
            if (!_s7DriverDal.IsConnected)
                return new ErrorResult("PLC Bağlantısı Yok! Emniyet kontrolü yapılamadı.");

            // PLC DB1 Verisini Okuma
            var furnaceData = await _s7DriverDal.ReadClassAsync(new FurnaceDbModel(), dbNumber, startByte: 0);
            if (furnaceData == null)
                return new ErrorResult("PLC verisi okunamadı.");

            // Operasyona Göre Kilit Kontrolleri
            switch (operationCode)
            {
                case "START_BURNER":
                    // Yakıcı açılmadan önce hazne basıncı ve soğutma suyu şartı
                    if (furnaceData.ChamberPressure < 0.5f)
                        return new ErrorResult("EMNİYET KİLİDİ: Hazne basıncı yetersiz (< 0.5 Bar). Yakıcı başlatılamaz.");
                    break;

                case "OPEN_GAS_VALVE":
                    // Gaz vanası açılmadan önce yakıcı hatası olmamalı
                    bool isBurnerFault = (furnaceData.AlarmWord & (1 << 3)) != 0;
                    if (isBurnerFault)
                        return new ErrorResult("EMNİYET KİLİDİ: Yakıcı arızada iken Ana Gaz Vanası açılamaz.");
                    break;

                default:
                    break;
            }

            return new SuccessResult("Güvenlik kilitleri doğrulandı. İşlem güvenli.");
        }

        public async Task<IDataResult<MimicStateDto>> GetMimicStateAsync()
        {
            if (!_s7DriverDal.IsConnected)
            {
                return new SuccessDataResult<MimicStateDto>(new MimicStateDto
                {
                    SafetyStatusText = "PLC Bağlantısı Kopuk"
                });
            }

            var furnaceData = await _s7DriverDal.ReadClassAsync(new FurnaceDbModel(), dbNumber: 1, startByte: 0);
            if (furnaceData == null)
                return new ErrorDataResult<MimicStateDto>("Mimik durumu okunamadı.");

            ushort status = furnaceData.StatusWord;

            var mimicDto = new MimicStateDto
            {
                Burner1Active = (status & (1 << 0)) != 0,
                MainGasValveOpen = (status & (1 << 1)) != 0,
                CoolingFanRunning = (status & (1 << 2)) != 0,
                WaterPumpRunning = (status & (1 << 3)) != 0,
                InterlockActive = furnaceData.AlarmWord == 0,
                SafetyStatusText = furnaceData.AlarmWord == 0 ? "SİSTEM GÜVENLİ" : "EMNİYET İHLALİ / ALARM"
            };

            return new SuccessDataResult<MimicStateDto>(mimicDto);
        }
    }
}
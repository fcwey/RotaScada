using Business.Abstract;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class PlcManager : IPlcService
    {
        private readonly IS7DriverDal _s7DriverDal;

        public PlcManager(IS7DriverDal s7DriverDal)
        {
            _s7DriverDal = s7DriverDal;
        }

        public IResult ConnectPlc(string ipAddress)
        {
            var result = _s7DriverDal.Connect(ipAddress, CpuType.S71200, 0, 1);
            if (result)
                return new SuccessResult("PLC Bağlantısı Başarılı");

            return new ErrorResult("PLC'ye Bağlanılamadı");
        }

        public IResult DisconnectPlc()
        {
            _s7DriverDal.Disconnect();
            return new SuccessResult("PLC Bağlantısı Kesildi");
        }

        [LogAspect(typeof(FileLogger))]
        public IDataResult<float> ReadTemperature(int dbNumber, int startByte)
        {
            try
            {
                var rawValue = _s7DriverDal.ReadTag(DataType.DataBlock, dbNumber, startByte, VarType.Real);
                if (rawValue == null) return new ErrorDataResult<float>("Veri okunamadı");

                float temp = Convert.ToSingle(rawValue);
                return new SuccessDataResult<float>(temp);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<float>($"Hata: {ex.Message}");
            }
        }

        public IResult WriteSetpoint(int dbNumber, int startByte, float value)
        {
            // Setpoint Sınır Kontrolü (Business Rule)
            if (value < 0 || value > 1200)
            {
                return new ErrorResult("Setpoint değeri 0 - 1200 °C sınırları dışında.");
            }

            try
            {
                _s7DriverDal.WriteTag(DataType.DataBlock, dbNumber, startByte, value);
                return new SuccessResult("Setpoint PLC'ye yazıldı.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Yazma Hatası: {ex.Message}");
            }
        }
    }
}

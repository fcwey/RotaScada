using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IPlcService
    {
        IResult ConnectPlc(string ipAddress);
        IResult DisconnectPlc();
        IDataResult<float> ReadTemperature(int dbNumber, int startByte);
        IResult WriteSetpoint(int dbNumber, int startByte, float value);
    }
}


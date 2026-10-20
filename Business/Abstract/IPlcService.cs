using Core.Utilities.Results;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IPlcService
    {
        Task<IResult> ConnectPlcAsync(string ipAddress);
        IResult DisconnectPlc();
        Task<IDataResult<float>> ReadTemperatureAsync(int dbNumber, int startByte);
        Task<IResult> WriteSetpointAsync(int dbNumber, int startByte, float value);
        Task<IResult> PulseBitAsync(DataType dataType, int dbNumber, int startByte, int bitNumber, int durationMs = 500);
    }
}


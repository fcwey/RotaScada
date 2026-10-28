using Core.Utilities.Results;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IInterlockService
    {
        // Kritik bir manuel operasyon öncesi PLC emniyet şartlarını denetler
        Task<IResult> ValidateOperationSafetyAsync(string operationCode, int dbNumber = 1);

        // Mimik ekrandaki bileşenlerin anlık durumunu paket halinde getirir
        Task<IDataResult<MimicStateDto>> GetMimicStateAsync();
    }
}

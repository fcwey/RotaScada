using Core.Utilities.Results;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
namespace Business.Abstract
{
    public interface IAlarmService
    {
        // PLC'den okunan anlık alarm bit haritasını (Tag -> IsActive) işler
        Task<IResult> ProcessAlarmBitsAsync(Dictionary<string, bool> alarmStateMap);

        // Arayüzde gösterilecek aktif alarmları getirir
        Task<IDataResult<List<ActiveAlarmDto>>> GetActiveAlarmsAsync();

        // Operatörün bir alarmı görmesini/onaylamasını sağlar (ACK)
        Task<IResult> AcknowledgeAlarmAsync(int alarmId, int userId, string? userNote = null);

        // Tarih ve önceliğe göre geçmiş alarm kayıtlarını filtreler
        Task<IDataResult<List<AlarmHistoryDto>>> GetAlarmHistoryAsync(DateTime start, DateTime end, int? priorityLevel = null);
    }
}

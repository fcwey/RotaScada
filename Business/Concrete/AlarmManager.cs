using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AlarmManager : IAlarmService
    {
        private readonly IAlarmDal _alarmDal;

        public AlarmManager(IAlarmDal alarmDal)
        {
            _alarmDal = alarmDal;
        }

        public async Task<IResult> ProcessAlarmBitsAsync(Dictionary<string, bool> alarmStateMap)
        {
            var activeAlarms = await _alarmDal.GetListAsync(a => a.IsActive);

            foreach (var (tagName, isRaised) in alarmStateMap)
            {
                var existingAlarm = activeAlarms.FirstOrDefault(a => a.TagName == tagName);

                // 1. Yeni Alarm Oluştu
                if (isRaised && existingAlarm == null)
                {
                    var newAlarm = new Alarm
                    {
                        TagName = tagName,
                        Description = GetDescriptionByTag(tagName),
                        PriorityLevel = GetPriorityByTag(tagName),
                        TriggeredAt = DateTime.Now,
                        IsActive = true,
                        IsAcknowledged = false
                    };
                    await _alarmDal.AddAsync(newAlarm);
                }
                // 2. Alarm Pasife Düştü (Resetted)
                else if (!isRaised && existingAlarm != null)
                {
                    existingAlarm.IsActive = false;
                    existingAlarm.ResolvedAt = DateTime.Now;
                    await _alarmDal. UpdateAsync(existingAlarm);
                }
            }

            return new SuccessResult("Alarm bitleri başarıyla işlendi.");
        }

        public async Task<IDataResult<List<ActiveAlarmDto>>> GetActiveAlarmsAsync()
        {
            var activeAlarms = await _alarmDal.GetListAsync(a => a.IsActive);
            var dtos = activeAlarms.Select(a => new ActiveAlarmDto
            {
                Id = a.Id,
                TagName = a.TagName,
                Description = a.Description,
                PriorityLevel = a.PriorityLevel,
                TriggeredAt = a.TriggeredAt,
                IsAcknowledged = a.IsAcknowledged
            }).ToList();

            return new SuccessDataResult<List<ActiveAlarmDto>>(dtos);
        }

        public async Task<IResult> AcknowledgeAlarmAsync(int alarmId, int userId, string? userNote = null)
        {
            var alarm = await _alarmDal.GetAsync(a => a.Id == alarmId);
            if (alarm == null)
                return new ErrorResult("Alarm kaydı bulunamadı.");

            if (alarm.IsAcknowledged)
                return new ErrorResult("Bu alarm zaten onaylanmış.");

            alarm.IsAcknowledged = true;
            alarm.AcknowledgedAt = DateTime.Now;
            alarm.AcknowledgedByUserId = userId;
            alarm.UserNote = userNote;

            await _alarmDal.UpdateAsync(alarm);
            return new SuccessResult("Alarm başarıyla onaylandı (ACK).");
        }

        public async Task<IDataResult<List<AlarmHistoryDto>>> GetAlarmHistoryAsync(DateTime start, DateTime end, int? priorityLevel = null)
        {
            var history = await _alarmDal.GetListAsync(a =>
                a.TriggeredAt >= start &&
                a.TriggeredAt <= end &&
                (!priorityLevel.HasValue || a.PriorityLevel == priorityLevel.Value));

            var dtos = history.Select(a => new AlarmHistoryDto
            {
                Id = a.Id,
                TagName = a.TagName,
                Description = a.Description,
                PriorityLevel = a.PriorityLevel,
                TriggeredAt = a.TriggeredAt,
                AcknowledgedAt = a.AcknowledgedAt,
                ResolvedAt = a.ResolvedAt,
                UserNote = a.UserNote
            }).ToList();

            return new SuccessDataResult<List<AlarmHistoryDto>>(dtos);
        }

        private string GetDescriptionByTag(string tag) => tag switch
        {
            "ERR_ZONE1_OVERTEMP" => "Bölge 1 Aşırı Sıcaklık İhlali",
            "ERR_PRESSURE_LOW" => "Hazne Basıncı Düşük",
            "ERR_BURNER_FAULT" => "Yakıcı Arızası / Alev Kopması",
            _ => "Tanımsız Sistem Alarımı"
        };

        private int GetPriorityByTag(string tag) => tag switch
        {
            "ERR_ZONE1_OVERTEMP" => 3, // Critical
            "ERR_BURNER_FAULT" => 3,   // Critical
            "ERR_PRESSURE_LOW" => 2,   // Warning
            _ => 1                     // Info
        };
    }
}

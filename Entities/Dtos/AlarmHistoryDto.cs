using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class AlarmHistoryDto : IDto
    {
        public int Id { get; set; }                             // Geçmiş alarm kaydının benzersiz kimliği
        public string TagName { get; set; } = string.Empty;     // Alarmı tetikleyen PLC tag adı
        public string Description { get; set; } = string.Empty; // Alarmın açıklama metni
        public int PriorityLevel { get; set; }                  // Alarm öncelik seviyesi (1: Bilgi, 2: Uyarı, 3: Kritik)
        public DateTime TriggeredAt { get; set; }               // Alarmın meydana geldiği zaman
        public DateTime? AcknowledgedAt { get; set; }           // Operatörün alarmı onayladığı (ACK) zaman damgası
        public DateTime? ResolvedAt { get; set; }               // Alarmın PLC tarafında pasife düştüğü / düzeldiği zaman
        public string? UserNote { get; set; }                   // Alarm onaylanırken operatörün girdiği açıklama notu
    }
}

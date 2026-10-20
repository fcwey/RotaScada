using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
   
    public class ActiveAlarmDto : IDto
    {
        public int Id { get; set; }                             // Alarm kaydının benzersiz kimlik numarası
        public string TagName { get; set; } = string.Empty;     // Alarmı tetikleyen PLC tag adı (Örn: "ERR_ZONE1_OVERTEMP")
        public string Description { get; set; } = string.Empty; // Alarmın insan tarafından okunabilir açıklama metni
        public int PriorityLevel { get; set; }                  // Alarm öncelik seviyesi (1: Bilgi, 2: Uyarı, 3: Kritik)
        public DateTime TriggeredAt { get; set; }               // Alarmın ilk oluştuğu tarih ve saat
        public bool IsAcknowledged { get; set; }                // Operatörün alarmı görüp onaylama (ACK) durumu
    }
}

using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Alarm : IEntity
    {
        public int Id { get; set; }
        public string TagName { get; set; } = string.Empty;     // Örn: "ERR_ZONE1_OVERTEMP"
        public string Description { get; set; } = string.Empty; // Örn: "Bölge 1 Aşırı Sıcaklık Alarımı"
        public int PriorityLevel { get; set; }                  // 1: Info, 2: Warning, 3: Critical
        public DateTime TriggeredAt { get; set; }               // Alarm oluşma zamanı
        public DateTime? AcknowledgedAt { get; set; }           // Operatör onay zamanı
        public DateTime? ResolvedAt { get; set; }               // Alarmın düzelme zamanı
        public bool IsActive { get; set; }                      // Alarm şu an devam ediyor mu?
        public bool IsAcknowledged { get; set; }                // Operatör gördü mü (ACK)?
        public int? AcknowledgedByUserId { get; set; }          // Onaylayan kullanıcı
        public string? UserNote { get; set; }
    }
}

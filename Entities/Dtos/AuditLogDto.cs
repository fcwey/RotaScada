using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class AuditLogDto : IDto
    {
        public long Id { get; set; }                            // Log kaydının benzersiz kimlik numarası
        public int? UserId { get; set; }                         // İşlemi gerçekleştiren operatörün kullanıcı ID'si
        public string ActionType { get; set; } = string.Empty;  // Eylem türü (Örn: "SETPOINT_CHANGE", "RECIPE_DOWNLOAD")
        public string Description { get; set; } = string.Empty; // Yapılan eylemin detaylı açıklaması
        public string? OldValue { get; set; }                   // Değiştirilmeden önceki eski parametre değeri
        public string? NewValue { get; set; }                   // Değiştirildikten sonraki yeni parametre değeri
        public string ClientIp { get; set; } = string.Empty;     // İşlemin yapıldığı istemci bilgisayarın IP adresi
        public DateTime Timestamp { get; set; }                 // Eylemin gerçekleştiği zaman damgası
    }
}

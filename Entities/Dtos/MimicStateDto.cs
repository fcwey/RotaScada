using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class MimicStateDto : IDto
    {
        public bool Burner1Active { get; set; }        // Yakıcı 1 Çalışıyor mu?
        public bool MainGasValveOpen { get; set; }     // Ana Gaz Vanası Açık mı?
        public bool CoolingFanRunning { get; set; }    // Soğutma Fanı Çalışıyor mu?
        public bool WaterPumpRunning { get; set; }     // Su Pompası Çalışıyor mu?
        public bool InterlockActive { get; set; }      // Güvenlik Kilidi Devrede mi?
        public string SafetyStatusText { get; set; } = string.Empty; // Güvenlik Durum Metni
    }
}

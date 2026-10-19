using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Recipe : IEntity
    {
        public int Id { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal TargetTemperature { get; set; }          // Hedef Sıcaklık (°C)
        public int SoakTimeMinutes { get; set; }                // Bekleme Süresi (Dakika)
        public decimal HeatingRampRate { get; set; }            // Isınma Hızı (°C/dakika)
        public decimal GasFlowSetPoint { get; set; }            // Gaz Debisi Set Değeri
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}

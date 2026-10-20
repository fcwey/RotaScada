using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class TrendPointDto : IDto
    {
        public DateTime Timestamp { get; set; }                 // Verinin okunduğu zaman damgası (Grafik X ekseni)
        public float Value { get; set; }                        // Sensörden okunan sayısal değer (Grafik Y ekseni)
    }
}

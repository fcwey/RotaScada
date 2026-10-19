using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class ProcessDataLog : IEntity
    {
        public long Id { get; set; }
        public int? BatchId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public float Zone1Temperature { get; set; }
        public float Zone2Temperature { get; set; }
        public float ChamberPressure { get; set; }
        public float GasFlowRate { get; set; }
        public bool BurnerStatus { get; set; }
    }
}

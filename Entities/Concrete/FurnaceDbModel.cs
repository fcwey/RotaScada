using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class FurnaceDbModel : IEntity
    {
        public float Zone1Temperature { get; set; }  // DB1.CBD0  (Real)
        public float Zone2Temperature { get; set; }  // DB1.CBD4  (Real)
        public float ChamberPressure { get; set; }   // DB1.CBD8  (Real)
        public float GasFlowRate { get; set; }       // DB1.CBD12 (Real)
        public ushort StatusWord { get; set; }       // DB1.DBW16 (Word)
        public ushort AlarmWord { get; set; }        // DB1.DBW18 (Word)
        public bool HeartbeatAck { get; set; }       // DB1.DBX20.0 (Bool)
    }
}

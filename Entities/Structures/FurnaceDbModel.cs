using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

//FurnaceDbModel Amacı: Bu sınıf veritabanında bir tablo değildir. Siemens PLC'nin DB1 (DB_PLC_To_HMI) bellek bloğunu S7.Net kütüphanesiyle tek seferde okumak
//(ReadClassAsync) için kullanılan bir PLC Bellek Haritası (PLC Mapping Model) sınıfıdır.
namespace Entities.Structures
{
    public class FurnaceDbModel 
    {
        // Siemens PLC DB1 (DB_PLC_To_HMI) Canlı Telemetri Bellek Haritası
        public float Zone1Temperature { get; set; }  // DB1.CBD0  (Real)
        public float Zone2Temperature { get; set; }  // DB1.CBD4  (Real)
        public float ChamberPressure { get; set; }   // DB1.CBD8  (Real)
        public float GasFlowRate { get; set; }       // DB1.CBD12 (Real)
        public ushort StatusWord { get; set; }       // DB1.DBW16 (Word)
        public ushort AlarmWord { get; set; }        // DB1.DBW18 (Word)
        public bool HeartbeatAck { get; set; }       // DB1.DBX20.0 (Bool)
    }
}

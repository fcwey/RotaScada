using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class PlcTagConfig : IEntity
    {
        public int Id { get; set; }
        public string TagName { get; set; } = string.Empty; // Örn: Temp_Zone1
        public string DbType { get; set; } = "DataBlock";   // DataBlock, Memory, Input, Output
        public int DbNumber { get; set; }                   // Örn: DB1 -> 1
        public int StartByte { get; set; }                  // Örn: 0
        public string VarType { get; set; } = "Real";       // Real, Bit, Int, DInt
        public int BitAddress { get; set; }                 // Bit ise (0-7)
    }
}

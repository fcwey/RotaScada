namespace Entities.Structures
{
    // TIA Portal "Step" STRUCT Karşılığı (42 Byte / Adım)
    public class PlcStepStruct
    {
        public short StepType { get; set; }           // Int (2 Byte)
        public float Vac_SetPoint { get; set; }        // Real (4 Byte)
        public uint Vac_Timeout { get; set; }          // Time (4 Byte - ms)
        public float TargetTemp { get; set; }          // Real (4 Byte)
        public uint StepTime { get; set; }             // Time (4 Byte - ms)
        public float GSoak { get; set; }               // Real (4 Byte)
        public float N2_Flow { get; set; }             // Real (4 Byte)
        public float NH3_Flow { get; set; }            // Real (4 Byte)
        public float dNH3_Flow { get; set; }           // Real (4 Byte)
        public float Su_Flow { get; set; }             // Real (4 Byte)
        public float Oxygen_Set { get; set; }          // Real (4 Byte)
    }
}
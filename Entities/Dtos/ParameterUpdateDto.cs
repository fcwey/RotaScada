namespace Entities.Dtos
{
    public class ParameterUpdateDto
    {
        public string ParameterKey { get; set; } = string.Empty; // Örn: "MAX_TEMP_LIMIT"
        public float NewValue { get; set; }
        public int DbNumber { get; set; } = 1;                   // PLC DB No
        public int StartByte { get; set; }                      // PLC Byte Adresi
    }
}
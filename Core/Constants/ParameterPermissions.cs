namespace Core.Constants
{
    public static class ParameterPermissions
    {
        // Seviye 1: Temel Operasyonel Ayarlar (Operatör ve Üstü)
        public const string SetpointBasicWrite = "Settings.Basic.Write";

        // Seviye 2: Kalibrasyon ve Gaz Parametreleri (Bakımcı / Mühendis)
        public const string CalibrationWrite = "Settings.Calibration.Write";
        public const string GasFlowLimitsWrite = "Settings.GasFlowLimits.Write";

        // Seviye 3: Kritik Emniyet ve Fırın Sıcaklık Limitleri (Sadece Kıdemli Mühendis / Admin)
        public const string SafetyLimitsWrite = "Settings.SafetyLimits.Write";
        public const string PlcIpConfigWrite = "Settings.PlcConfig.Write";
    }
}
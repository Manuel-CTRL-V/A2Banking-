using System.Configuration;

namespace ATM.Kiosk.Services.Configuration
{
    /// <summary>
    /// Configuración del kiosko ATM leída desde app.config.
    /// Cada instalación física del ATM tiene su propio app.config
    /// con su URL de servidor y su identificador único.
    ///
    /// app.config esperado:
    /// <appSettings>
    ///   <add key="BankApiBaseUrl"  value="http://192.168.1.100:5000/api/" />
    ///   <add key="ATM_Identifier"  value="ATM-CENTRO-01" />
    ///   <add key="LogFilePath"     value="C:\BankLogs\atm_log.txt" />
    ///   <add key="RequestTimeout"  value="30" />
    /// </appSettings>
    /// </summary>
    public static class KioskConfig
    {
        public static string BankApiBaseUrl =>
            ConfigurationManager.AppSettings["BankApiBaseUrl"]
            ?? throw new ConfigurationErrorsException(
                "Falta 'BankApiBaseUrl' en app.config.");

        public static string ATM_Identifier =>
            ConfigurationManager.AppSettings["ATM_Identifier"]
            ?? "ATM-DEFAULT";

        public static string LogFilePath =>
            ConfigurationManager.AppSettings["LogFilePath"]
            ?? "atm_log.txt";

        /// <summary>Timeout en segundos para llamadas al servidor.</summary>
        public static int RequestTimeoutSeconds
        {
            get
            {
                var raw = ConfigurationManager.AppSettings["RequestTimeout"];
                return int.TryParse(raw, out int seconds) ? seconds : 30;
            }
        }
    }
}

using System.Configuration;

namespace BackOffice.Services.Configuration
{
    public static class BackOfficeConfig
    {
        public static string BankApiBaseUrl =>
            ConfigurationManager.AppSettings["BankApiBaseUrl"]
            ?? throw new ConfigurationErrorsException(
                "Falta 'BankApiBaseUrl' en app.config.");

        public static string LogFilePath =>
            ConfigurationManager.AppSettings["LogFilePath"] ?? "backoffice_log.txt";

        public static int RequestTimeoutSeconds
        {
            get
            {
                var raw = ConfigurationManager.AppSettings["RequestTimeout"];
                return int.TryParse(raw, out int s) ? s : 30;
            }
        }

    }
}

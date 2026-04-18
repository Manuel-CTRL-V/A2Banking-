using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using BankAPI.DataAccess.Implementations;

namespace BankAPI.DataAccess.Configuration
{
    /// <summary>
    /// Lee configuración desde appsettings.json.
    /// Se inicializa una sola vez en Program.cs antes de que
    /// arranque la aplicación. El resto del sistema la consume
    /// como propiedades estáticas — sin inyección de dependencias.
    /// </summary>
    public static class DatabaseConfig
    {
        public static string ConnectionString { get; private set; }
        public static string LogFilePath      { get; private set; }

        public static decimal DailyDepositLimit   { get; private set; }
        public static decimal DailyWithdrawLimit  { get; private set; }
        public static decimal DailyTransferLimit  { get; private set; }

        public static void Initialize(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("BankDB")
                ?? throw new InvalidOperationException(
                    "Falta la cadena de conexión 'BankDB' en appsettings.json.");

            LogFilePath = config["AppSettings:LogFilePath"] ?? "api_log.txt";

            var limitsSection = config.GetSection("TransactionLimits");
            
            var logger = Logger.Instance;
            
            if (limitsSection.Exists() && !string.IsNullOrEmpty(limitsSection["DailyDepositLimit"]))
            {
                DailyDepositLimit  = ParseInvariantDecimal(limitsSection["DailyDepositLimit"], 100000m);
                DailyWithdrawLimit = ParseInvariantDecimal(limitsSection["DailyWithdrawLimit"], 50000m);
                DailyTransferLimit = ParseInvariantDecimal(limitsSection["DailyTransferLimit"], 50000m);
            }
            else
            {
                DailyDepositLimit = 100000m;
                DailyWithdrawLimit = 50000m;
                DailyTransferLimit = 50000m;
                
                logger.LogWarning("TransactionLimits no encontrado en config, usando valores por defecto");
            }
            
            logger.LogInfo($"[INIT] DailyDepositLimit={DailyDepositLimit:N2} RD$, DailyWithdrawLimit={DailyWithdrawLimit:N2} RD$, DailyTransferLimit={DailyTransferLimit:N2} RD$");
            logger.LogInfo($"[INIT] Raw config values - Deposit: '{limitsSection["DailyDepositLimit"]}', Withdraw: '{limitsSection["DailyWithdrawLimit"]}', Transfer: '{limitsSection["DailyTransferLimit"]}'");
        }

        private static decimal ParseInvariantDecimal(string value, decimal defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            value = value.Replace(",", ".").Trim();
            
            if (decimal.TryParse(value, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return defaultValue;
        }
    }
}

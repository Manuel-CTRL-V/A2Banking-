using ATM.Shared.DTOs.Notifications;
using BankAPI.DataAccess.Implementations;
using BankAPI.DataAccess.Interfaces;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BankAPI.Business.Services
{
    /// <summary>
    /// Envía notificaciones por Email (SMTP) y SMS (Twilio)
    /// tras operaciones bancarias exitosas.
    ///
    /// Configuración requerida en appsettings.json:
    /// "Smtp": {
    ///   "Host":     "smtp.gmail.com",
    ///   "Port":     587,
    ///   "Username": "tu_cuenta@gmail.com",
    ///   "Password": "tu_app_password"
    /// },
    /// "Twilio": {
    ///   "AccountSid": "ACxxx...",
    ///   "AuthToken":  "xxx...",
    ///   "FromNumber": "+1234567890"
    /// }
    /// </summary>
    public class NotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly Logger _logger;

        // SMTP
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        // Twilio
        private readonly string _twilioSid;
        private readonly string _twilioToken;
        private readonly string _twilioFrom;

        public NotificationService(
            INotificationRepository repo,
            Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _repo = repo;
            _logger = Logger.Instance;

            _smtpHost = config["Smtp:Host"] ?? "";
            _smtpPort = int.Parse(config["Smtp:Port"] ?? "587");
            _smtpUser = config["Smtp:Username"] ?? "";
            _smtpPass = config["Smtp:Password"] ?? "";

            _twilioSid = config["Twilio:AccountSid"] ?? "";
            _twilioToken = config["Twilio:AuthToken"] ?? "";
            _twilioFrom = config["Twilio:FromNumber"] ?? "";
        }

        // API pública

        public NotificationSettingsResponse GetSettings(int accountId)
            => _repo.GetSettings(accountId);

        public void SaveSettings(NotificationSettingsRequest request)
            => _repo.SaveSettings(request);

        /// <summary>
        /// Llamado por los controllers tras una transacción exitosa.
        /// No lanza excepción — el fallo de notificación no debe
        /// revertir la transacción bancaria.
        /// </summary>
        public void NotifyTransaction(
            int accountId,
            string holderName,
            string operationType,
            decimal amount,
            decimal newBalance,
            decimal commission = 0)
        {
            try
            {
                var settings = _repo.GetSettings(accountId);

                if (!settings.EmailEnabled && !settings.SmsEnabled) return;

                var subject = "A2 Banking — " + operationType + " realizado";
                var body = BuildMessage(
                    holderName, operationType, amount, newBalance, commission);

                if (settings.EmailEnabled &&
                    !string.IsNullOrEmpty(settings.EmailAddress))
                    SendEmail(settings.EmailAddress, subject, body);

                if (settings.SmsEnabled &&
                    !string.IsNullOrEmpty(settings.PhoneNumber))
                    SendSms(settings.PhoneNumber, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Error al enviar notificación para cuenta " +
                    accountId + ": " + ex.Message,
                    accountId: accountId);
            }
        }

        // Email vía SMTP

        private void SendEmail(string to, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpHost, _smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);

                var msg = new MailMessage(_smtpUser, to, subject, body);
                client.Send(msg);

                _logger.LogInfo("Email enviado a: " + to);
            }
        }

        // SMS vía Twilio

        private void SendSms(string to, string body)
        {
            TwilioClient.Init(_twilioSid, _twilioToken);

            // SMS tiene límite de 160 caracteres — truncar si es necesario
            var smsBody = body.Length > 160
                ? body.Substring(0, 157) + "..."
                : body;

            MessageResource.Create(
                to: new Twilio.Types.PhoneNumber(to),
                from: new Twilio.Types.PhoneNumber(_twilioFrom),
                body: smsBody);

            _logger.LogInfo("SMS enviado a: " + to);
        }

        // Mensaje

        private static string BuildMessage(
            string holderName,
            string operationType,
            decimal amount,
            decimal newBalance,
            decimal commission)
        {
            var msg = "Hola " + holderName + ",\n\n" +
                      "Se realizó un " + operationType + " en su cuenta A2 Banking.\n" +
                      "Monto: " + amount.ToString() + "RD$ \n" +
                      "Saldo actual: " + newBalance.ToString() + "RD$ \n";

            if (commission > 0)
                msg += "Comisión aplicada: " + commission.ToString() + "RD$ \n";

            msg += "\nFecha: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                   "\n\nSi no reconoce esta operación, contacte al banco.";

            return msg;
        }
    }

}

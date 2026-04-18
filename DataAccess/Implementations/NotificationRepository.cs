using ATM.Shared.DTOs.Notifications;
using BankAPI.DataAccess.Helpers;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.DataAccess.Implementations
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public NotificationSettingsResponse GetSettings(int accountId)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetNotificationSettings", conn)
                    .With("@AccountId", accountId)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        return new NotificationSettingsResponse
                        {
                            AccountId = accountId,
                            EmailEnabled = false,
                            SmsEnabled = false
                        };

                    return new NotificationSettingsResponse
                    {
                        AccountId = r.GetInt("AccountId"),
                        EmailEnabled = r.GetBool("EmailEnabled"),
                        EmailAddress = r.GetString("EmailAddress"),
                        SmsEnabled = r.GetBool("SmsEnabled"),
                        PhoneNumber = r.GetString("PhoneNumber")
                    };
                }
            });
        }

        public void SaveSettings(NotificationSettingsRequest request)
        {
            Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_SaveNotificationSettings", conn)
                    .With("@AccountId", request.AccountId)
                    .With("@EmailEnabled", request.EmailEnabled)
                    .WithNullable("@EmailAddress", request.EmailAddress)
                    .With("@SmsEnabled", request.SmsEnabled)
                    .WithNullable("@PhoneNumber", request.PhoneNumber)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    // solo ejecutar
                }
            });
        }
    }
}

using ATM.Shared.DTOs.Notifications;

namespace BankAPI.DataAccess.Interfaces
{
    public interface INotificationRepository
    {
        NotificationSettingsResponse GetSettings(int accountId);
        void SaveSettings(NotificationSettingsRequest request);
    }
}

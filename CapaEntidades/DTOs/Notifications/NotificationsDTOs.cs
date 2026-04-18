using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Shared.DTOs.Notifications
{
    public class NotificationSettingsRequest
    {
        public int AccountId { get; set; }
        public bool EmailEnabled { get; set; }
        public string EmailAddress { get; set; }
        public bool SmsEnabled { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class NotificationSettingsResponse
    {
        public int AccountId { get; set; }
        public bool EmailEnabled { get; set; }
        public string EmailAddress { get; set; }
        public bool SmsEnabled { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}

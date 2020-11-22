using System;
using Newtonsoft.Json;
using PayOn.Core;

namespace PayOn.Models
{
    [Serializable]
    public class PaymentRequest : PaymentBase
    {
        public PaymentRequest() : base()
        {
            CreateRegistration = false;
        }

        public void SetRecurringPayment(bool initial)
        {
            RecurringType = initial ? "INITIAL" : "REPEATED";
            CreateRegistration = initial;
        }

        [JsonProperty("recurringType")]
        public string RecurringType { get; set; }

        [JsonProperty("createRegistration")]
        public bool CreateRegistration { get; set; }

        [JsonProperty("shopperResultUrl")]
        public string ShopperResultUrl { get; set; }

        [JsonProperty("notificationUrl")]
        public string NotificationUrl { get; set; }
    }
}

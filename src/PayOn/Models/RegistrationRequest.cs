using System;
using Newtonsoft.Json;
using PayOn.Core;

namespace PayOn.Models
{
    [Serializable]
    public class RegistrationRequest : MessageBase
    {
        public RegistrationRequest() : base()
        {
            CreateRegistration = false;
        }

        public void SetRecurringPayment(bool initial)
        {
            RecurringType = initial ? "INITIAL" : "REPEATED";
            CreateRegistration = initial;
            PaymentType = "DB";
        }

        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("registrationId")]
        public string RegistrationId { get; set; }

        [JsonProperty("recurringType")]
        public string RecurringType { get; set; }

        [JsonProperty("createRegistration")]
        public bool CreateRegistration { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}

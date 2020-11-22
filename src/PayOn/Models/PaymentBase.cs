using System;
using Newtonsoft.Json;
using PayOn.Core;

namespace PayOn.Models
{
    [Serializable]
    public class PaymentBase : MessageBase
    {
        public PaymentBase()
        {
            PaymentType = "DB";
        }

        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("descriptor")]
        public string Descriptor { get; set; }

        [JsonProperty("threeDSecure")]
        public ThreeDSecure ThreeDSecure { get; set; }

        [JsonProperty("customParameters")]
        public CustomParameters CustomParameters { get; set; }
    }
}

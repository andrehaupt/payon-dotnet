using System;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class ThreeDSecurePaymentBrand
    {
        [JsonProperty("requestorId")]
        public string RequestorId { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}

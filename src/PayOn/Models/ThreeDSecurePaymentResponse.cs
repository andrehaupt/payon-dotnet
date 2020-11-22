using System;
using Newtonsoft.Json;
using PayOn.Core;

namespace PayOn.Models
{
    [Serializable]
    public class ThreeDSecurePaymentResponse : PaymentBase
    {
        [JsonProperty("registrationId")]
        public string RegistrationId { get; set; }
        
        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("redirect")]
        public Redirect Redirect { get; set; }
    }
}

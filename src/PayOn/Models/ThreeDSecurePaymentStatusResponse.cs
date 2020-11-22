using System;
using Newtonsoft.Json;
using PayOn.Core;

namespace PayOn.Models
{
    [Serializable]
    public class ThreeDSecurePaymentStatusResponse : PaymentBase
    {
        [JsonProperty("result")]
        public Result Result { get; set; }
    }
}

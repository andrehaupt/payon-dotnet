using System;
using Newtonsoft.Json;
using PayOn.Core;

namespace PayOn.Models
{
    [Serializable]
    public class ThreeDSecureResponse : PaymentBase
    {
        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("risk")]
        public Risk Risk { get; set; }
    }
}

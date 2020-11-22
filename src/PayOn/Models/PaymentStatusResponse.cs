using System;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class PaymentStatusResponse : PaymentBase
    {
        [JsonProperty("result")]
        public Result Result { get; set; }
    }
}

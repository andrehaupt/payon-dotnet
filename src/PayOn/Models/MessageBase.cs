using System;
using Newtonsoft.Json;
using PayOn.Core;

namespace PayOn.Models
{
    [Serializable]
    public class MessageBase
    {
        public override string ToString() 
        {
            return JsonConvert.SerializeObject(this.CreateMaskedClone(), Formatting.Indented);
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("paymentBrand")]
        public string PaymentBrand { get; set; }

        [JsonProperty("card")]
        public CardAccount Card { get; set; }

        [JsonProperty("buildNumber")]
        public string BuildNumber { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("ndc")]
        public string Ndc { get; set; }
    }
}

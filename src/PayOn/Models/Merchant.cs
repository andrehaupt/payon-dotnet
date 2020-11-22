using System;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class Merchant
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}

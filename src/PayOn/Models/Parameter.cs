using System;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}

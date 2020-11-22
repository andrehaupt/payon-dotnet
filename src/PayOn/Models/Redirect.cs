using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class Redirect
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("parameters")]
        public ICollection<Parameter> Parameter { get; set; }
    }
}
using System;
using Newtonsoft.Json;

namespace PayOn.Models
{

    [Serializable]
    public class Risk
    {
        [JsonProperty("score")]
        public long Score { get; set; }
    }
}

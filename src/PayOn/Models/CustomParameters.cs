using System;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class CustomParameters
    {
        [JsonProperty("CTPE_DESCRIPTOR_TEMPLATE")]
        public string CtpeDescriptorTemplate { get; set; }
    }
}

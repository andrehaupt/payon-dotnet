﻿using System;
using Newtonsoft.Json;
using PayOn.Core;

namespace PayOn.Models
{
    [Serializable]
    public class RegistrationResponse : MessageBase
    {
        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("risk")]
        public Risk Risk { get; set; }

        [JsonProperty("redirect")]
        public Redirect Redirect { get; set; }
    }
}

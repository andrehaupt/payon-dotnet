using System;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class ThreeDSecurePaymentBrands
    {
        [JsonProperty("visa")]
        public ThreeDSecurePaymentBrand Visa { get; set; }

        [JsonProperty("mastercard")]
        public ThreeDSecurePaymentBrand Mastercard { get; set; }

        [JsonProperty("diners")]
        public ThreeDSecurePaymentBrand Diners { get; set; }

        [JsonProperty("amex")]
        public ThreeDSecurePaymentBrand Amex { get; set; }

        [JsonProperty("jcb")]
        public ThreeDSecurePaymentBrand Jcb { get; set; }

        [JsonProperty("dankort")]
        public ThreeDSecurePaymentBrand Dankort { get; set; }

        [JsonProperty("bcmc")]
        public ThreeDSecurePaymentBrand Bcmc { get; set; }
    }
}

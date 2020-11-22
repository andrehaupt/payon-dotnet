using System;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class ThreeDSecure
    {
        [JsonProperty("eci")]
        public string Eci { get; set; }

        [JsonProperty("verificationId")]
        public string VerificationId { get; set; }

        [JsonProperty("xid")]
        public string Xid { get; set; }

        [JsonProperty("enrollmentStatus")]
        public string EnrollmentStatus { get; set; }

        [JsonProperty("authenticationStatus")]
        public string AuthenticationStatus { get; set; }

        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; }

        [JsonProperty("v1")]
        public ThreeDSecurePaymentBrands V1 { get; set; }
    }
}

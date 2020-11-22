using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PayOn.Models
{
    [Serializable]
    public class Result
    {
        private const string ApprovedPattern = @"^(000\.000\.|000\.100\.1|000\.[36])";
        private const string ManualReviewPattern = @"^(000\.400\.0[^3]|000\.400\.100)";
        private const string PendingPattern = @"^(000\.200)";
        private const string TransientPattern = @"^(800\.400\.5|100\.400\.500)";
        private const string DeclinedPattern = 
            // 3D Secure and Intercard risk checks
            @"^(000\.400\.[1][0-9][1-9]|000\.400\.2)" + "|" +
            // Rejections by the external bank or similar payment system
            @"^(800\.[17]00|800\.800\.[123])" + "|" +
            // Due to communication errors
            @"^(900\.[1234]00|000\.400\.030)" + "|" +
            // System errors
            @"^(800\.[56]|999\.|600\.1|800\.800\.8)" + "|" +
            // Error in asynchronous workflow
            @"^(100\.39[765])" + "|" +
            // Soft decline
            @"^(300\.100\.100)" + "|" +
            // Checks by external risk systems
            @"^(100\.400\.[0-3]|100\.38|100\.370\.100|100\.370\.11)" + "|" +
            // Address validation
            @"^(800\.400\.1)" + "|" +
            // 3D Secure
            @"^(800\.400\.2|100\.380\.4|100\.390)" + "|" +
            // Blacklist validation
            @"^(100\.100\.701|800\.[32])" + "|" +
            // Risk validation
            @"^(800\.1[123456]0)" + "|" +
            // Configuration validation
            @"^(600\.[23]|500\.[12]|800\.121)" + "|" +
            // Registration validation
            @"^(100\.[13]50)" + "|" +
            // Job validation
            @"^(100\.250|100\.360)" + "|" +
            // Reference validation
            @"^(700\.[1345][05]0)" + "|" +
            // Format validation
            @"^(200\.[123]|100\.[53][07]|800\.900|100\.[69]00\.500)" + "|" +
            // Address validation
            @"^(100\.800)" + "|" +
            // Contact validation
            @"^(100\.[97]00)" + "|" +
            // Account validation
            @"^(100\.100|100.2[01])" + "|" +
            // Amount validation
            @"^(100\.55)" + "|" +
            // Risk management
            @"^(100\.380\.[23]|100\.380\.101)" + "|" +
            // Chargebacks
            @"^(000\.100\.2)";

        public ResultStatus GetResultStatus() { 
            switch(Code) {
                case string code when new Regex(ApprovedPattern).IsMatch(code):
                    return ResultStatus.Approved;
                case string code when new Regex(ManualReviewPattern).IsMatch(code):
                    return ResultStatus.ManualReview;
                case string code when new Regex(PendingPattern).IsMatch(code):
                    return ResultStatus.Pending;
                case string code when new Regex(TransientPattern).IsMatch(code):
                    return ResultStatus.Transient;
                case string code when new Regex(DeclinedPattern).IsMatch(code):
                    return ResultStatus.Declined;
                default:
                    return ResultStatus.Unknown;
            }
        }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parameterErrors")]
        public Parameter[] ParameterErrors { get; set; }
    }
}

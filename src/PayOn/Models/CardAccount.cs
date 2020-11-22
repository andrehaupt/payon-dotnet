using System;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PayOn.Models
{
    //Card Account
    // The card data structure holds all information regarding a credit or debit card account.
    // 
    // Parameter	Description	Format	Required
    // card.holder	Holder of the credit card account	A128
    // {3,128}	Optional
    // card.number	The PAN or account number of the card.	N19
    // [0-9]{12,19}	Required
    // card.expiryMonth	The expiry month of the card.	N2
    // (0[1-9]|1[0-2])	Required
    // card.expiryYear	The expiry year of the card.	N4
    // (19|20)([0-9]{2})	Required
    // card.cvv	The card security code or CVV	N4
    // [0-9]{3,4}	Conditional
    [Serializable]
    public class CardAccount
    {
        [JsonProperty("holder")]
        public string Holder { get; set; }

        internal void Mask()
        {
            if (!string.IsNullOrWhiteSpace(Number)) 
            {
                StringBuilder sb = new StringBuilder(Number);

                for (int i = 6; i < Number.Length - 4; i++) 
                {
                    sb[i] = '*';
                }

                Number = sb.ToString();
            }

            if (!string.IsNullOrWhiteSpace(ExpiryYear))
            {
                ExpiryYear = Regex.Replace(ExpiryYear, @"\d", "*");
            }

            if (!string.IsNullOrWhiteSpace(ExpiryMonth))
            {
                ExpiryMonth = Regex.Replace(ExpiryMonth, @"\d", "*");
            }

            if (!string.IsNullOrWhiteSpace(Cvv))
            {
                Cvv = Regex.Replace(Cvv, @"\d", "*");
            }
        }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("bin")]
        public string Bin { get; set; }

        [JsonProperty("last4Digits")]
        public string Last4Digits { get; set; }

        [JsonProperty("expiryMonth")]
        public string ExpiryMonth { get; set; }

        [JsonProperty("expiryYear")]
        public string ExpiryYear { get; set; }

        [JsonProperty("cvv")]
        public string Cvv { get; set; }

        [JsonProperty("threeDSecure")]
        public bool ThreeDSecure { get; set; }
        
        [JsonProperty("paymentBrand")]
        public string PaymentBrand { get; set; }
    }
}

using System;
namespace PayOn.Examples.Models
{
    public class PaymentViewModel
    {
        public string Id { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentBrand { get; set; }
        public string CardNumber { get; set; }
        public string CardHolder { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
        public string CardCvv { get; set; }
        public string RegistrationId { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
    }
}

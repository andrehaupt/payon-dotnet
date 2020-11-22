using System;
using PayOn.Models;

namespace PayOn.Tests
{
    public static class TestHelper
    {
        public static DateTime FutureDate = DateTime.Now.AddYears(1);

        public static CardAccount[] GetTestPaymentCardAccounts()
        {
            return new[]
            {
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "4012888888881881",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = true,
                    PaymentBrand = "VISA"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "4111111111111111",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = false,
                    PaymentBrand = "VISA"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "4012888888881881",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = false,
                    PaymentBrand = "VISAELECTRON"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "5105105105105100",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = false,
                    PaymentBrand = "MASTER"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "4111111111111111",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = false,
                    PaymentBrand = "CARTEBLEUE"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "4242424242424242",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = false,
                    PaymentBrand = "VISA"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "5454545454545454",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = false,
                    PaymentBrand = "MASTER"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "5221266361111726",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = true,
                    PaymentBrand = "MASTER"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "4341793000000034",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = true,
                    PaymentBrand = "VISA"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "5506750000000149",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = true,
                    PaymentBrand = "MASTER"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "2223000010043807",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = true,
                    PaymentBrand = "MASTER"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "2223000010043815",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = true,
                    PaymentBrand = "MASTER"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "2223000010043823",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = true,
                    PaymentBrand = "MASTER"
                },
                new CardAccount
                {
                    Holder = "Jane Jones",
                    Number = "2223000010043831",
                    ExpiryMonth = FutureDate.ToString("MM"),
                    ExpiryYear = FutureDate.ToString("yyyy"),
                    Cvv = "123",
                    ThreeDSecure = true,
                    PaymentBrand = "MASTER"
                },
            };
        }
    }
}

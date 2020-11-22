using System;
using System.Linq;
using Newtonsoft.Json;
using PayOn.Infrastructure.Extensions;
using PayOn.Models;
using Xunit;
using Xunit.Abstractions;

namespace PayOn.Tests
{
    public class PayOnClientTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly PayOnHttpClient _payOnHttpClient;

        /// <summary>
        /// It is important to note that we have two test modes available to cause requests to be sent to our connector simulator or to the connector's own test platform, as required:
        /// * testMode=EXTERNAL causes test transactions to be forwarded to the processor's test system for 'end-to-end' testing
        /// * testMode=INTERNAL causes transactions to be sent to our simulators, which is useful when switching to the live endpoint for connectivity testing.
        /// 
        /// If no testMode parameter is sent, testMode= INTERNAL is the default behaviour
        /// </summary>
        private const string TestMode = "";
        private const string EntityId_ThreeDPayment = "xxx";
        private const string EntityId_Recurring = "xxx";
        private const string UserId = "xxx";
        private const string Password = "xxx";
        private const string BaseUrl = "https://test.oppwa.com";

        public PayOnClientTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _payOnHttpClient = new PayOnHttpClient();
        }

        [Fact]
        public void ThreeDSecurePayment()
        {
            PayOnClient threeDSecureClient = new PayOnClient(EntityId_ThreeDPayment, UserId, Password, BaseUrl, TestMode);
            CardAccount[] cardAccounts = TestHelper.GetTestPaymentCardAccounts()
                .Where(x => x.ThreeDSecure)
                .ToArray();

            foreach (var cardAccount in cardAccounts)
            {
                int idx = Array.FindIndex(cardAccounts, x => x == cardAccount) + 1;
                Log($"\nTest card: {idx}/{cardAccounts.Count()}");

                ThreeDSecurePaymentRequest request = new ThreeDSecurePaymentRequest
                {
                    PaymentBrand = cardAccount.PaymentBrand,
                    Amount = "123.45",
                    Card = cardAccount,
                    Currency = "ZAR",
                    Timestamp = DateTime.Now.ToIso8601String(),
                    ShopperResultUrl = "https://newco/payment/confirm"
                };

                Log("3D Secure Request", JsonConvert.SerializeObject(request));
                ThreeDSecurePaymentResponse response = threeDSecureClient.RequestThreeDSecurePayment(request);
                Log("3D Secure Response", JsonConvert.SerializeObject(response));

                Assert.Equal(ResultStatus.Pending, response.Result.GetResultStatus());

                AcsRequest acsRequest = _payOnHttpClient.GetAcsRequestAsync(response).Result;

                Log("ACS Request", JsonConvert.SerializeObject(acsRequest));
                Assert.True(acsRequest.IsValid);
                AcsResponse acsResponse = _payOnHttpClient.PostAcsRequestAsync(acsRequest).Result;
                Log("ACS Response", JsonConvert.SerializeObject(acsResponse));

                Assert.True(acsResponse.IsValid);

                AcsRedirectResponse acsRedirectResponse = _payOnHttpClient.GetAcsRedirectResponseAsync(acsResponse).Result;

                Log("ACS Redirect Response", JsonConvert.SerializeObject(acsRedirectResponse));
                Assert.True(acsRedirectResponse.IsValid);
                ThreeDSecurePaymentStatusResponse paymentStatus = threeDSecureClient.RequestThreeDSecurePaymentStatus(acsRedirectResponse.Id);
                Log("3D Secure Payment Status", JsonConvert.SerializeObject(paymentStatus));

                Assert.Equal(ResultStatus.Approved, paymentStatus.Result.GetResultStatus());
            }
        }

        [Fact]
        public void ThreeDSecureTokenizationWithRecurringPayment()
        {
            PayOnClient threeDSecureClient = new PayOnClient(EntityId_ThreeDPayment, UserId, Password, BaseUrl, TestMode);
            PayOnClient recurringPaymentClient = new PayOnClient(EntityId_Recurring, UserId, Password, BaseUrl, TestMode);
            CardAccount[] cardAccounts = TestHelper.GetTestPaymentCardAccounts();

            foreach (var cardAccount in cardAccounts)
            {
                int idx = Array.FindIndex(cardAccounts, x => x == cardAccount) + 1;
                Log($"\nTest card: {idx}/{cardAccounts.Count()}: {cardAccount.Number}");

                PaymentRequest request = new PaymentRequest
                {
                    PaymentBrand = cardAccount.PaymentBrand,
                    Amount = "1.00",
                    Currency = "ZAR",
                    Card = cardAccount,
                    PaymentType = "DB",
                    RecurringType = "INITIAL",
                    CreateRegistration = true,
                    ShopperResultUrl = "https://newco/payment/confirm",
                    Timestamp = DateTime.Now.ToIso8601String()
                };

                Log("3D Secure Request with Tokenization", JsonConvert.SerializeObject(request));
                PaymentResponse response = threeDSecureClient.RequestPayment(request);
                Log("3D Secure Response with Tokenization", JsonConvert.SerializeObject(response));

                string registrationId = response.RegistrationId;

                Assert.NotNull(registrationId);

                // If ACS is required
                if (response.Result.GetResultStatus() == ResultStatus.Pending)
                {
                    PaymentStatusResponse paymentResult = RequestPaymentStatus(threeDSecureClient, cardAccount, response.Redirect);
                    Assert.Equal(paymentResult?.Result?.GetResultStatus(), ResultStatus.Approved);
                }
                else
                {
                    Assert.Equal(response.Result.GetResultStatus(), ResultStatus.Approved);
                }

                // Recurring transaction
                RegistrationRequest recurringRequest = new RegistrationRequest
                {
                    PaymentBrand = cardAccount.PaymentBrand,
                    Amount = "123.45",
                    Currency = "ZAR",
                    RecurringType = "REPEATED",
                    PaymentType = "DB",
                    CreateRegistration = false,
                    RegistrationId = registrationId
                };

                Log("Initial Recurring Registration Request", JsonConvert.SerializeObject(recurringRequest));
                RegistrationResponse recurringResponse = recurringPaymentClient.RequestRegistration(recurringRequest);
                Log("Repeat Recurring Registration Response", JsonConvert.SerializeObject(recurringResponse));

                Assert.Equal(recurringResponse.Result.GetResultStatus(), ResultStatus.Approved);
            }
        }

        [Fact]
        public void ThreeDSecureTokenizationWithReversal()
        {
            PayOnClient threeDSecureClient = new PayOnClient(EntityId_ThreeDPayment, UserId, Password, BaseUrl, TestMode);
            PayOnClient recurringPaymentClient = new PayOnClient(EntityId_Recurring, UserId, Password, BaseUrl, TestMode);
            CardAccount[] cardAccounts = TestHelper.GetTestPaymentCardAccounts();

            foreach (var cardAccount in cardAccounts)
            {
                int idx = Array.FindIndex(cardAccounts, x => x == cardAccount) + 1;
                Log($"\nTest card: {idx}/{cardAccounts.Count()}: {cardAccount.Number}");

                PaymentRequest request = new PaymentRequest
                {
                    PaymentBrand = cardAccount.PaymentBrand,
                    Amount = "1.00",
                    Currency = "ZAR",
                    Card = cardAccount,
                    PaymentType = "DB",
                    RecurringType = "INITIAL",
                    CreateRegistration = true,
                    ShopperResultUrl = "https://newco/payment/confirm",
                    Timestamp = DateTime.Now.ToIso8601String()
                };

                Log("3D Secure Request with Tokenization", JsonConvert.SerializeObject(request));
                PaymentResponse response = threeDSecureClient.RequestPayment(request);
                Log("3D Secure Response with Tokenization", JsonConvert.SerializeObject(response));

                string id = response.Id;

                Assert.NotNull(id);

                string registrationId = response.RegistrationId;

                Assert.NotNull(registrationId);


                // If ACS is required
                if (response.Result.GetResultStatus() == ResultStatus.Pending)
                {
                    PaymentStatusResponse paymentResult = RequestPaymentStatus(threeDSecureClient, cardAccount, response.Redirect);
                    Assert.Equal(paymentResult?.Result?.GetResultStatus(), ResultStatus.Approved);
                    id = paymentResult.Id;
                }
                else
                {
                    Assert.Equal(response.Result.GetResultStatus(), ResultStatus.Approved);
                }

                // Recurring transaction
                PaymentRequest paymentRequest = new PaymentRequest
                {
                    PaymentType = "RV",
                };

                Log("Payment Reversal Request Id", id);
                PaymentResponse recurringResponse = threeDSecureClient.RequestPaymentReversal(id);
                Log("Payment Reversal Response", JsonConvert.SerializeObject(recurringResponse));

                Assert.Equal(recurringResponse.Result.GetResultStatus(), ResultStatus.Approved);
            }
        }

        private PaymentStatusResponse RequestPaymentStatus(PayOnClient threeDSecureClient, CardAccount cardAccount, Redirect redirect)
        {
            AcsRequest acsRequest = _payOnHttpClient.GetAcsRequestAsync(redirect).Result;

            Log("ACS Request", JsonConvert.SerializeObject(acsRequest));
            AcsResponse acsResponse = _payOnHttpClient.PostAcsRequestAsync(acsRequest).Result;
            Log("ACS Response", JsonConvert.SerializeObject(acsResponse));
            
            AcsRedirectResponse acsRedirectResponse = _payOnHttpClient.GetAcsRedirectResponseAsync(acsResponse).Result;

            Log("ACS Redirect Response", JsonConvert.SerializeObject(acsRedirectResponse));
            PaymentStatusResponse paymentStatus = threeDSecureClient.RequestPaymentStatus(acsRedirectResponse.Id);
            Log("3D Secure Payment Status", JsonConvert.SerializeObject(paymentStatus));

            return paymentStatus;
        }

        private void Log(string message)
        {
            _testOutputHelper.WriteLine(message);
            Console.WriteLine(message);
        }

        private void Log(string message, string content)
        {
            Log($"\n{message}:\n----------------------\n{content}");
        }
    }
}

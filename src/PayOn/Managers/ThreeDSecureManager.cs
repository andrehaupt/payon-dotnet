using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayOn.Models;

namespace PayOn.Managers
{
    internal class ThreeDSecureManager : BaseManager<ThreeDSecurePaymentResponse>
    {
        private const string PaymentPath = "/v1/threeDSecure";
        private readonly string _testMode;

        internal ThreeDSecureManager(string baseUrl, string entityId, Authentication authentication, string testMode, ILogger logger) :
            base(baseUrl, entityId, authentication, logger)
        {
            _testMode = testMode;
        }

        internal ThreeDSecurePaymentResponse RequestPayment(ThreeDSecurePaymentRequest request)
        {
            string requestData = CreateRequestData(request);
            return Request(PaymentPath, requestData);
        }

        // TODO: The design here is not in line with the usage of generics in the base class. Rethink
        internal ThreeDSecurePaymentStatusResponse RequestPaymentStatus(string transactionId)
        {
            ThreeDSecurePaymentStatusResponse response;
            string statusPath = $"{PaymentPath}/{transactionId}";
            Dictionary<string, string> parameters = new Dictionary<string, string> {
                { "entityId", EntityId}
            };

            try
            {
                response = JsonConvert.DeserializeObject<ThreeDSecurePaymentStatusResponse>(GetRequest(statusPath, parameters));
            }
            catch (WebException ex)
            {
                LogError("Response not received", ex);
                response = CreateFailedResponse<ThreeDSecurePaymentStatusResponse>(ex);
                LogResponse(JsonConvert.SerializeObject(response));
            }

            return response;
        }

        internal string CreateRequestData(ThreeDSecurePaymentRequest request) =>
            $"entityId={EntityId}" +
            $"&amount={request.Amount}" +
            $"&currency={request.Currency}" +
            $"&paymentBrand={request.PaymentBrand}" +
            (
                string.IsNullOrEmpty(request.RecurringType) || request.RecurringType == "INITIAL" ?
                    $"&card.number={request.Card.Number}" +
                    $"&card.holder={request.Card.Holder}" +
                    $"&card.expiryMonth={request.Card.ExpiryMonth}" +
                    $"&card.expiryYear={request.Card.ExpiryYear}" +
                    $"&card.cvv={request.Card.Cvv}" :
                    string.Empty
            ) +
            (
                !string.IsNullOrEmpty(request.RecurringType) ?
                    $"&recurringType={request.RecurringType}" :
                    string.Empty
            ) +
            (
                request.CreateRegistration ? 
                    $"&createRegistration=true" :
                    string.Empty
            ) +
            $"&shopperResultUrl={request.ShopperResultUrl}" +
            (
                !string.IsNullOrWhiteSpace(_testMode) ? 
                    $"&testMode={_testMode}" : 
                    ""
            );
    }
}

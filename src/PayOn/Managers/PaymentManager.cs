using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayOn.Models;

namespace PayOn.Managers
{
    internal class PaymentManager : BaseManager<PaymentResponse>
    {
        private const string PaymentPath = "/v1/payments";
        private const string PaymentReversalPath = "/v1/payments/{0}";
        private readonly string _testMode;

        internal PaymentManager(string baseUrl, string entityId, Authentication authentication, string testMode, ILogger logger) :
            base(baseUrl, entityId, authentication, logger)
        {
            _testMode = testMode;
        }

        internal PaymentResponse Request(PaymentRequest request)
        {
            return Request(PaymentPath, CreateRequestData(request));
        }

        internal PaymentResponse RequestPaymentReversal(string transactionId)
        {
            PaymentRequest request = new PaymentRequest
            {
                PaymentType = "RV"
            };

            string path = string.Format(PaymentReversalPath, transactionId);

            return Request(path, CreateRequestData(request));
        }

        // TODO: The design here is not in line with the usage of generics in the base class. Rethink
        internal PaymentStatusResponse RequestPaymentStatus(string transactionId)
        {
            PaymentStatusResponse response;
            string statusPath = $"{PaymentPath}/{transactionId}";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "entityId", EntityId}
            };

            try
            {
                response = JsonConvert.DeserializeObject<PaymentStatusResponse>(GetRequest(statusPath, parameters));
            }
            catch (WebException ex)
            {
                LogError("Response not received", ex);
                response = CreateFailedResponse<PaymentStatusResponse>(ex);
                LogResponse(JsonConvert.SerializeObject(response));
            }

            return response;
        }

        internal string CreateRequestData(PaymentRequest request) =>
            $"entityId={EntityId}" +
            (
                !string.IsNullOrEmpty(request.Amount) ?
                    $"&amount={request.Amount}" :
                    string.Empty
            ) +
            (
                !string.IsNullOrEmpty(request.Currency) ?
                    $"&currency={request.Currency}" :
                    string.Empty
            ) +
            (
                !string.IsNullOrEmpty(request.PaymentBrand) ?
                    $"&paymentBrand={request.PaymentBrand}" :
                    string.Empty
            ) +
            $"&paymentType={request.PaymentType}" +
            (
                (request.Card != null) && (string.IsNullOrEmpty(request.RecurringType) || request.RecurringType == "INITIAL") ?
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
            (
                !string.IsNullOrEmpty(request.ShopperResultUrl) ?
                    $"&shopperResultUrl={request.ShopperResultUrl}" :
                    string.Empty
            ) +
            (
                !string.IsNullOrWhiteSpace(_testMode) ?
                    $"&testMode={_testMode}" :
                    string.Empty
            );
    }
}
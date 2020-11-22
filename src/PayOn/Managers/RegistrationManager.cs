using System;
using Microsoft.Extensions.Logging;
using PayOn.Models;

namespace PayOn.Managers
{
    internal class RegistrationManager : BaseManager<RegistrationResponse>
    {
        private const string RegistrationPath = "/v1/registrations";
        private const string RegistrationPaymentsPath = "/v1/registrations/{0}/payments";
        private readonly ILogger _logger;
        private readonly string _testMode;

        internal RegistrationManager(string baseUrl, string entityId, Authentication authentication, string testMode, ILogger logger) : 
            base(baseUrl, entityId, authentication, logger)
        {
            _testMode = testMode;
        }

        internal RegistrationResponse Request(RegistrationRequest request)
        {
            if (request.CreateRegistration && string.IsNullOrWhiteSpace(request.RegistrationId))
            {
                throw new ArgumentException("Repeat payments should have a RegistrationId.");
            }

            string path = request.CreateRegistration ? 
                RegistrationPath :
                string.Format(RegistrationPaymentsPath, request.RegistrationId);

            return Request(path, CreateRequestData(request));
        }

        internal string CreateRequestData(RegistrationRequest request) =>
            $"entityId={EntityId}" +
            (
                request.CreateRegistration ?
                    $"&paymentBrand={request.PaymentBrand}" +
                    $"&card.number={request.Card.Number}" +
                    $"&card.holder={request.Card.Holder}" +
                    $"&card.expiryMonth={request.Card.ExpiryMonth}" +
                    $"&card.expiryYear={request.Card.ExpiryYear}" +
                    $"&card.cvv={request.Card.Cvv}" +
                    $"&createRegistration=true" +
                    $"&paymentType=DB" :
                    $"&amount={request.Amount}" +
                    $"&currency={request.Currency}"
            ) +
            $"&paymentType={request.PaymentType}" +
            $"&recurringType={request.RecurringType}" +
            (
                !string.IsNullOrWhiteSpace(_testMode) ? 
                    $"&testMode={_testMode}" : 
                    ""
            );
    }
}
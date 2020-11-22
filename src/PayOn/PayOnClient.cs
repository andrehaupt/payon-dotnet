using System;
using Microsoft.Extensions.Logging;
using PayOn.Infrastructure.Extensions;
using PayOn.Managers;
using PayOn.Models;

namespace PayOn
{
    public class PayOnClient
    {
        private readonly string _entityId;
        private readonly string userId;
        private readonly string password;
        private readonly Authentication _authentication;
        private readonly string _baseUrl;
        private readonly string _testMode;
        private readonly ILogger _logger;

        public PayOnClient(string entityId,
            string userId,
            string password,
            string baseUrl,
            string testMode = "",
            ILogger logger = null)
        {
            if (string.IsNullOrWhiteSpace(entityId))
                throw new ArgumentException("The entityId cannot be the empty string.", nameof(entityId));

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("The userId cannot be the empty string.", nameof(userId));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("The password cannot be the empty string.", nameof(password));

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("The url cannot be the empty string.", nameof(baseUrl));

            _entityId = entityId;
            this.userId = userId;
            this.password = password;
            _baseUrl = baseUrl;
            _logger = logger;
            _testMode = testMode;
            _authentication = new Authentication(userId, password);
        }

        public PaymentResponse RequestPayment(PaymentRequest request)
        {
            PaymentManager paymentManager = new PaymentManager(_baseUrl, _entityId, _authentication, _testMode, _logger);
            return paymentManager.Request(request);
        }

        public ThreeDSecurePaymentResponse RequestThreeDSecurePayment(ThreeDSecurePaymentRequest request)
        {
            ThreeDSecureManager paymentManager = new ThreeDSecureManager(_baseUrl, _entityId, _authentication, _testMode, _logger);
            return paymentManager.RequestPayment(request);
        }

        public ThreeDSecurePaymentStatusResponse RequestThreeDSecurePaymentStatus(string transactionId)
        {
            ThreeDSecureManager paymentManager = new ThreeDSecureManager(_baseUrl, _entityId, _authentication, _testMode, _logger);
            return paymentManager.RequestPaymentStatus(transactionId);
        }

        public RegistrationResponse RequestRegistration(RegistrationRequest request)
        {
            RegistrationManager registrationManager = new RegistrationManager(_baseUrl, _entityId, _authentication, _testMode, _logger);
            return registrationManager.Request(request);
        }

        public PaymentStatusResponse RequestPaymentStatus(string transactionId)
        {
            PaymentManager paymentManager = new PaymentManager(_baseUrl, _entityId, _authentication, _testMode, _logger);
            return paymentManager.RequestPaymentStatus(transactionId);
        }

        public PaymentResponse RequestPaymentReversal(string transactionId)
        {
            PaymentManager paymentManager = new PaymentManager(_baseUrl, _entityId, _authentication, _testMode, _logger);
            return paymentManager.RequestPaymentReversal(transactionId);
        }
    }
}

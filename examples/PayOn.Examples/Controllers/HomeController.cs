using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayOn.Examples.Models;
using PayOn.Models;
using PayOn.Infrastructure.Extensions;

namespace PayOn.Examples.Controllersz
{
    public class HomeController : Controller
    {
        private static string CachedRegistrationId;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ThreeDSecure()
        {
            DateTime FutureDate = DateTime.Now.AddYears(1);

            PaymentViewModel model = new PaymentViewModel
            {
                Amount = "123.45",
                Currency = "ZAR",
                PaymentBrand = "VISA",
                CardNumber = "4711100000000000",
                CardHolder = "John Smith",
                CardExpiryMonth = FutureDate.ToString("MM"),
                CardExpiryYear = FutureDate.ToString("yyyy"),
                CardCvv = "123"
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ThreeDSecure(PaymentViewModel model)
        {
            PayOnClient client = new PayOnClient(Settings.EntityIdThreeDSecure, Settings.UserId, Settings.Password, Settings.BaseUrl);
            ThreeDSecurePaymentRequest request = new ThreeDSecurePaymentRequest
            {
                PaymentBrand = model.PaymentBrand,
                Amount = model.Amount,
                Card = new CardAccount
                {
                    Holder = model.CardHolder,
                    Number = model.CardNumber,
                    ExpiryMonth = model.CardExpiryMonth,
                    ExpiryYear = model.CardExpiryYear,
                    Cvv = model.CardCvv,
                    ThreeDSecure = true,
                    PaymentBrand = model.PaymentBrand
                },
                Currency = "ZAR",
                Timestamp = DateTime.Now.ToIso8601String(),
                ShopperResultUrl = $"{Request.Scheme}://{Request.Host}/Home/ThreeDSecureConfirm"
            };

            try
            {
                ThreeDSecurePaymentResponse response = client.RequestThreeDSecurePayment(request);

                if (response.Result.GetResultStatus() == ResultStatus.Approved)
                {
                    model.Status = $"Card not enrolled for 3D Secure. Result code: {response.Result.Code}. Result Description: {response.Result.Description}";
                    return View(model);
                }

                if (response.Result.GetResultStatus() == ResultStatus.Declined)
                {
                    model.Status = $"Declined. Result code: {response.Result.Code}. Result Description: {response.Result.Description}";
                    return View(model);
                }

                if (response.Result.GetResultStatus() != ResultStatus.Pending)
                {
                    model.Status = $"Unexpected response. Result code: {response.Result.Code}. Result Description: {response.Result.Description}";
                    return View(model);
                }

                return Content(GetRedirectPageHtml(response), "text/html");
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to start 3D Secure Transaction", ex);
                model.Status = $"Unable to start 3D Secure Transaction. {ex}";
            }

            return View(model);
        }

        public IActionResult ThreeDSecureConfirm(string id, string resourcePath)
        {
            PayOnClient client = new PayOnClient(Settings.EntityIdThreeDSecure, Settings.UserId, Settings.Password, Settings.BaseUrl);
            PaymentViewModel model = new PaymentViewModel
            {
                Id = id
            };

            try
            {
                ThreeDSecurePaymentStatusResponse paymentStatus = client.RequestThreeDSecurePaymentStatus(id);
                model.Status = $"{Enum.GetName(typeof(ResultStatus), paymentStatus.Result.GetResultStatus())}. Result code: {paymentStatus.Result.Code}. Result Description: {paymentStatus.Result.Description}"; ;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to start 3D Secure Transaction", ex);
                model.Status = $"Unable to start 3D Secure Transaction. {ex}";
            }

            return View(model);
        }

        public IActionResult RecurringInitial()
        {
            DateTime FutureDate = DateTime.Now.AddYears(1);

            PaymentViewModel model = new PaymentViewModel
            {
                Amount = "123.45",
                Currency = "ZAR",
                PaymentBrand = "VISA",
                CardNumber = "4711100000000000",
                CardHolder = "John Smith",
                CardExpiryMonth = FutureDate.ToString("MM"),
                CardExpiryYear = FutureDate.ToString("yyyy"),
                CardCvv = "123",
                RegistrationId = "Submit to register card"
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult RecurringInitial(PaymentViewModel model)
        {
            PayOnClient client = new PayOnClient(Settings.EntityIdThreeDSecure, Settings.UserId, Settings.Password, Settings.BaseUrl);
            PaymentRequest request = new PaymentRequest
            {
                PaymentBrand = model.PaymentBrand,
                Amount = model.Amount,
                Card = new CardAccount
                {
                    Holder = model.CardHolder,
                    Number = model.CardNumber,
                    ExpiryMonth = model.CardExpiryMonth,
                    ExpiryYear = model.CardExpiryYear,
                    Cvv = model.CardCvv,
                    PaymentBrand = model.PaymentBrand
                },
                Currency = "ZAR",
                PaymentType = "DB",
                RecurringType = "INITIAL",
                CreateRegistration = true,
                Timestamp = DateTime.Now.ToIso8601String(),
                ShopperResultUrl = $"{Request.Scheme}://{Request.Host}/Home/RecurringInitialConfirm"
            };

            try
            {
                PaymentResponse response = client.RequestPayment(request);

                model.Id = response.Id;

                // Not enrolled for 3D Secure
                if (response.Result.GetResultStatus() == ResultStatus.Approved)
                {
                    return RedirectToAction("RedirectComplete", response.RegistrationId);
                }

                if (response.Result.GetResultStatus() == ResultStatus.Declined || response.Result.GetResultStatus() != ResultStatus.Pending)
                {
                    model.Status = $"{Enum.GetName(typeof(ResultStatus), response.Result.GetResultStatus())}. Result code: {response.Result.Code}. Result Description: {response.Result.Description}";
                    return View(model);
                }

                CachedRegistrationId = response.RegistrationId;

                return Content(GetRedirectPageHtml(response), "text/html");

            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to start Recurring Transaction", ex);
                model.Status = $"Unable to start Recurring Transaction. {ex}";
            }

            return View(model);
        }

        public IActionResult RecurringInitialConfirm(string id, string resourcePath)
        {
            PayOnClient client = new PayOnClient(Settings.EntityIdThreeDSecure, Settings.UserId, Settings.Password, Settings.BaseUrl);
            PaymentViewModel model = new PaymentViewModel
            {
                Id = id
            };

            try
            {
                PaymentStatusResponse paymentStatus = client.RequestPaymentStatus(id);
                model.Status = $"{Enum.GetName(typeof(ResultStatus), paymentStatus.Result.GetResultStatus())}. Result code: {paymentStatus.Result.Code}. Result Description: {paymentStatus.Result.Description}";
                model.RegistrationId = CachedRegistrationId;
                model.Amount = "234.56";
                model.Currency = "ZAR";
                model.PaymentBrand = paymentStatus.PaymentBrand;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to start 3D Secure Transaction", ex);
                model.Status = $"Unable to start 3D Secure Transaction. {ex}";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult RecurringRepeat(PaymentViewModel model)
        {
            PayOnClient client = new PayOnClient(Settings.EntityIdRecurring, Settings.UserId, Settings.Password, Settings.BaseUrl);
            RegistrationRequest recurringRequest = new RegistrationRequest
            {
                PaymentBrand = model.PaymentBrand,
                Amount = model.Amount,
                Currency = model.Currency,
                RecurringType = "REPEATED",
                PaymentType = "DB",
                CreateRegistration = false,
                RegistrationId = model.RegistrationId
            };

            try
            {
                RegistrationResponse response = client.RequestRegistration(recurringRequest);
                model.Id = response.Id;
                model.Status = $"{Enum.GetName(typeof(ResultStatus), response.Result.GetResultStatus())}. Result code: {response.Result.Code}. Result Description: {response.Result.Description}";

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to start 3D Secure Transaction", ex);
                model.Status = $"Unable to start 3D Secure Transaction. {ex}";
            }

            return View(model);
        }

        public IActionResult Reverse(string id)
        {
            PayOnClient client = new PayOnClient(Settings.EntityIdThreeDSecure, Settings.UserId, Settings.Password, Settings.BaseUrl);
            PaymentViewModel model = new PaymentViewModel();

            try
            {
                PaymentRequest paymentRequest = new PaymentRequest
                {
                    PaymentType = "RV",
                };
                PaymentResponse response = client.RequestPaymentReversal(id);

                model.Status = $"{Enum.GetName(typeof(ResultStatus), response.Result.GetResultStatus())}. Result code: {response.Result.Code}. Result Description: {response.Result.Description}";
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to start 3D Secure Transaction", ex);
                model.Status = $"Unable to start 3D Secure Transaction. {ex}";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Reverse(PaymentViewModel model)
        {
            return Reverse(model.Id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetRedirectPageHtml(PaymentResponse response)
        {
            return GetRedirectPageHtml(response.Redirect);
        }

        private string GetRedirectPageHtml(ThreeDSecurePaymentResponse response)
        {
            return GetRedirectPageHtml(response.Redirect);
        }

        private string GetRedirectPageHtml(Redirect redirect)
        {
            Dictionary<string, string> dict = redirect.Parameter
               .ToDictionary(x => x.Name, x => x.Value);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, redirect.Url)
            {
                Content = new FormUrlEncodedContent(dict)
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}

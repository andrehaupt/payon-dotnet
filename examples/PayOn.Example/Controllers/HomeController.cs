using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using PayOn.Example.Models;
using PayOn.Models;

namespace PayOn.Example.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            DateTime FutureDate = DateTime.Now.AddYears(1);

            PaymentViewModel model = new PaymentViewModel {
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
        public IActionResult Index(PaymentViewModel model)
        {
            PayOnClient client = new PayOnClient(Settings.EntityId_3DSecure, Settings.UserId, Settings.Password, Settings.BaseUrl);
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
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH\\:mm\\:ss.ffzzz", System.Globalization.CultureInfo.InvariantCulture),
                ShopperResultUrl = $"{Request.Scheme}://{Request.Host}/Home/Confirm"
            };

            ThreeDSecurePaymentResponse response = client.RequestThreeDSecurePayment(request);

            return Content(GetRedirectPageHtml(response), "text/html");
        }

        private string GetRedirectPageHtml(ThreeDSecurePaymentResponse threeDSecurePaymentResponse)
        {
            Dictionary<string, string> dict = threeDSecurePaymentResponse.Redirect.Parameter
               .ToDictionary(x => x.Name, x => x.Value);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, threeDSecurePaymentResponse.Redirect.Url)
            {
                Content = new FormUrlEncodedContent(dict)
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        public IActionResult Confirm(string id, string resourcePath)
        {
            PayOnClient client = new PayOnClient(Settings.EntityId_3DSecure, Settings.UserId, Settings.Password, Settings.BaseUrl);
            ThreeDSecurePaymentStatusResponse paymentStatus = client.RequestThreeDSecurePaymentStatus(id);
            PaymentViewModel model = new PaymentViewModel {
                Status = Enum.GetName(typeof(ResultStatus), paymentStatus.Result.GetResultStatus())
            };

            return View(model);
        }
        public IActionResult Recurring()
        {
            DateTime FutureDate = DateTime.Now.AddYears(1);

            PaymentViewModel model = new PaymentViewModel {
                Amount = "123.45",
                Currency = "ZAR",
                PaymentBrand = "VISA",
                CardNumber = "4711100000000000",
                CardHolder = "John Smith",
                CardExpiryMonth = FutureDate.ToString("MM"),
                CardExpiryYear = FutureDate.ToString("yyyy"),
                CardCvv = "123",
                RegistrationId = "Submit to register card",
                Status = "N/A"
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Recurring(PaymentViewModel model)
        {
            // TODO: Enable this again
            // PayOnClient client = new PayOnClient(Settings.EntityId_Recurring, Settings.UserId, Settings.Password, Settings.BaseUrl);
            // PaymentRequest request = new PaymentRequest
            // {
            //     PaymentBrand = model.PaymentBrand,
            //     Amount = model.Amount,
            //     Card = new CardAccount
            //     {
            //         Holder = model.CardHolder,
            //         Number = model.CardNumber,
            //         ExpiryMonth = model.CardExpiryMonth,
            //         ExpiryYear = model.CardExpiryYear,
            //         Cvv = model.CardCvv,
            //         ThreeDSecure = true,
            //         PaymentBrand = model.PaymentBrand
            //     },
            //     Currency = "ZAR",
            //     Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH\\:mm\\:ss.ffzzz", System.Globalization.CultureInfo.InvariantCulture)
            // };

            // PaymentResponse response = null;

            // // If first submission register card first
            // if (model.RegistrationId.Length != 32)
            // {
            //     response = client.RequestInitialRecurringPayment(request);
            //     model.RegistrationId = response.RegistrationId;
            //     model.Status = "Initial Registration and Payment - " + Enum.GetName(typeof(ResultStatus), response.Result.GetResultStatus());
            // }
            // else
            // {
            //     response = client.RequestRepeatRecurringPayment(request, model.RegistrationId);
            //     model.Status = "Repeat Recurring Payment - " + Enum.GetName(typeof(ResultStatus), response.Result.GetResultStatus());
            // }

            // ModelState.Clear();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

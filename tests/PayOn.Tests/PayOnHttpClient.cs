using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PayOn.Models;

namespace PayOn.Tests
{
    public class PayOnHttpClient {
        private static readonly HttpClient _client = new HttpClient();

        public PayOnHttpClient() {
        }

        public async Task<AcsRequest> GetAcsRequestAsync(ThreeDSecurePaymentResponse threeDSecurePaymentResponse) {
            return await GetAcsRequestAsync(threeDSecurePaymentResponse.Redirect);
        }
        
        public async Task<AcsRequest> GetAcsRequestAsync(PaymentResponse paymentResponse) {
            return await GetAcsRequestAsync(paymentResponse.Redirect);
        }
        
        public async Task<AcsRequest> GetAcsRequestAsync(Redirect redirect) {
            Dictionary<string, string> dict = redirect.Parameter
                .ToDictionary(x => x.Name, x => x.Value);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, redirect.Url) { 
                Content = new FormUrlEncodedContent(dict) 
            };
            
            LogRequest(request);

            HttpResponseMessage response = await client.SendAsync(request);
            string html = await response.Content.ReadAsStringAsync();
            HtmlDocument htmlDoc = new HtmlDocument(html);

            return new AcsRequest {
                FormActionUrl = htmlDoc.GetAttribute("form[name$=\"simulationForm\"]", "action"),
                Md = htmlDoc.GetValue("input[name$=\"MD\"]"),
                PaRes = htmlDoc.GetValue("input[name$=\"PaRes\"]"),
                Ndcid = htmlDoc.GetValue("input[name$=\"ndcid\"]"),
                ReturnCode = "000.000.000"
            };
        }

        public async Task<AcsResponse> PostAcsRequestAsync(AcsRequest acsRequest)
        {
            Dictionary<string, string> dict = new Dictionary<string, string> {
                {"MD", acsRequest.Md},
                {"PaRes", acsRequest.PaRes},
                {"ndcid", acsRequest.Ndcid},
                {"returnCode", acsRequest.ReturnCode}
            };

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, acsRequest.FormActionUrl) { 
                Content = new FormUrlEncodedContent(dict) 
            };
            
            LogRequest(request);

            HttpResponseMessage response = await client.SendAsync(request);
            string html = await response.Content.ReadAsStringAsync();
            HtmlDocument htmlDoc = new HtmlDocument(html);

            return new AcsResponse {
                RedirectUrl = htmlDoc.GetAttribute("form[name$=\"redirectToMerchant\"]", "action")
            };
        }

        public async Task<AcsRedirectResponse> GetAcsRedirectResponseAsync(AcsResponse acsResponse)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, acsResponse.RedirectUrl);
            
            LogRequest(request);

            HttpResponseMessage response = await client.SendAsync(request);
            string html = await response.Content.ReadAsStringAsync();
            HtmlDocument htmlDoc = new HtmlDocument(html);

            return new AcsRedirectResponse {
                Id = htmlDoc.GetValue("input[name$=\"id\"]")
            };
        }

        internal void LogRequest(HttpRequestMessage webRequest, string requestData = "")
        {
            string logString = $"{webRequest.Method} {webRequest.RequestUri}{Environment.NewLine}" + 
                $"Request Headers:{Environment.NewLine}" + 
                $"{webRequest.Headers}{Environment.NewLine}" +
                (!string.IsNullOrEmpty(requestData) ? $"Request Data: {requestData}" : string.Empty);
            Console.WriteLine(logString);
        }
    }
}
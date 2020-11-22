using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayOn.Models;

namespace PayOn.Managers
{
    internal class BaseManager<T> where T : class
    {
        internal readonly string BaseUrl;
        internal readonly string EntityId;
        internal readonly Authentication Authentication;
        internal readonly ILogger Logger;

        internal BaseManager(string baseUrl, string entityId, Authentication authentication, ILogger logger)
        {
            BaseUrl = baseUrl;
            EntityId = entityId;
            Authentication = authentication;
            Logger = logger;
        }

        internal T Request(string path, string requestData)
        {
            T response;

            try
            {
                response = JsonConvert.DeserializeObject<T>(PostRequest(path, requestData));
            }
            catch (WebException ex)
            {
                LogError("Response not received", ex);
                response = CreateFailedResponse<T>(ex);
                LogResponse(JsonConvert.SerializeObject(response));
            }

            return response;
        }

        internal string GetRequest(string path, Dictionary<string, string> parameters = null)
        {
            string response;
            string queryString = parameters != null ?
                string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}")) :
                string.Empty;
            string fullPath = !string.IsNullOrWhiteSpace(queryString) ?
                $"{BaseUrl}{path}?{queryString}" :
                $"{BaseUrl}{path}";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(fullPath);
            webRequest.Method = "GET";
            webRequest.Headers["Authorization"] = $"Bearer {Authentication.AccessToken}";
            webRequest.Timeout = 60000;

            LogRequest(webRequest);

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream ?? throw new InvalidOperationException());
                response = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }

            return response;
        }

        internal string PostRequest(string path, string requestData)
        {
            string response;
            byte[] buffer = Encoding.ASCII.GetBytes(requestData);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create($"{BaseUrl}{path}");
            webRequest.Method = "POST";
            webRequest.Headers["Authorization"] = $"Bearer {Authentication.AccessToken}";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Timeout = 60000;

            LogRequest(webRequest, requestData);

            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream ?? throw new InvalidOperationException());
                response = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }

            LogResponse(response);

            return response;
        }

        protected U CreateFailedResponse<U>(WebException ex) where U : class
        {
            using (var response = ((HttpWebResponse) ex.Response))
            using (var reader = new StreamReader(response?.GetResponseStream() ?? throw new IOException()))
            {
                string responseText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<U>(responseText);
            }
        }

        internal void LogRequest(HttpWebRequest webRequest, string requestData = "")
        {
            StringBuilder headerStr = new StringBuilder();

            for (int i = 0; i < webRequest.Headers.Count; i++)
            {
                headerStr.AppendLine($"{webRequest.Headers.AllKeys[i]}: {webRequest.Headers[i]}");
            }

            string logString = $"{webRequest.Method} {webRequest.RequestUri}{Environment.NewLine}" + 
                $"Request Headers:{Environment.NewLine}" + 
                $"{headerStr}{Environment.NewLine}" +
                (!string.IsNullOrEmpty(requestData) ? $"Request Data: {requestData}" : string.Empty);
            Logger?.LogInformation(logString);
            Console.WriteLine(logString);
        }

        internal void LogResponse(string response)
        {   
            string logString = $"Response Data: {response}";
            Logger?.LogInformation(logString);
            Console.WriteLine(logString);
        }

        internal void LogError(string message, Exception ex)
        {
            string logString = $"{message}\n{ex}";
            Logger?.LogError(ex, message);
            Console.WriteLine(logString);
        }
    }
}

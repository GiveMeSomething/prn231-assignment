using System;
using System.Net.Http.Headers;
using System.Text;
using FAPortal.Utils;
using Newtonsoft.Json.Linq;

namespace FAPortal.Client
{
    public class HttpHandler
    {
        public HttpClient Client { get; set; }

        private const string _basePath = "http://localhost:5000/api/";

        public HttpHandler()
        {
            Client = new();
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var uri = new Uri(_basePath + requestUri);
            var response = await Client.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T body, string token = "")
        {
            var jsonBody = CustomJson.Stringify(body);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Setup header
            httpContent.Headers.ContentType =
                new MediaTypeHeaderValue("application/json");

            if (!string.IsNullOrEmpty(token))
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var uri = new Uri(_basePath + requestUri);
            var response = await Client.PostAsync(uri, httpContent);

            return response;
        }
        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var uri = new Uri(_basePath + requestUri);
            var response = await Client.DeleteAsync(uri);
            return response;
        }
    }
}


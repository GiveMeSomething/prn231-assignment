using System;
using System.Net.Http.Headers;
using System.Text;
using FAPortal.Utils;

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

        public async Task<HttpResponseMessage> GetAsync<T>(string requestUri, T body, bool includeToken = false)
        {
            if (includeToken)
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", GetTokenForUser(1));
            }

            var uri = new Uri(_basePath + requestUri);
            var response = await Client.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T body, bool includeToken = false)
        {
            var jsonBody = CustomJson.Stringify(body);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Setup header
            httpContent.Headers.ContentType =
                new MediaTypeHeaderValue("application/json");

            if (includeToken)
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", GetTokenForUser(1));
            }

            var uri = new Uri(_basePath + requestUri);
            var response = await Client.PostAsync(uri, httpContent);

            return response;
        }

        private static string GetTokenForUser(int userId)
        {
            return "";
        }
    }
}


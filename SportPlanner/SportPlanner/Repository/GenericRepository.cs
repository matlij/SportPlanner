using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(SportPlanner.Repository.GenericRepository))]

namespace SportPlanner.Repository
{
    public class GenericRepository : IGenericRepository
    {
        public async Task<T> GetAsync<T>(string requestUri)
        {
            using (var client = GetHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);
                if (!response.IsSuccessStatusCode)
                    return default;

                var content = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(content);

                return result;
            }
        }

        public async Task<T> PostAsync<T>(string requestUri, T body)
        {
            using (var client = GetHttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(body));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(requestUri, content);

                if (!response.IsSuccessStatusCode)
                    return default;

                var responseContent = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(responseContent);

                return result;
            }
        }

        public async Task<bool> PutAsync<T>(string requestUri, T body)
        {
            using (var client = GetHttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(body));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PutAsync(requestUri, content);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> PatchAsync<T>(string requestUri, T body)
        {
            using (var client = GetHttpClient())
            {
                var method = new HttpMethod("PATCH");
                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var request = new HttpRequestMessage(method, requestUri)
                {
                    Content = content
                };

                var response = await client.SendAsync(request);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteAsync(string requestUri)
        {
            using (var client = GetHttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(requestUri);

                return response.IsSuccessStatusCode;
            }
        }

        private static HttpClient GetHttpClient()
        {
#if DEBUG
            HttpClientHandler insecureHandler = GetInsecureHandler();
            return new HttpClient(insecureHandler);
#else
            return new HttpClient();
#endif
        }

        private static HttpClientHandler GetInsecureHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (cert.Issuer.Equals("CN=localhost"))
                    {
                        return true;
                    }

                    return errors == System.Net.Security.SslPolicyErrors.None;
                }
            };
        }
    }
}

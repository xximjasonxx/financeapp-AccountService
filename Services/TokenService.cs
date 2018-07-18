
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

namespace AccountService.Services
{
    public static class TokenService
    {
        public static async Task<string> GetUserIdForToken(string token, TraceWriter log)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("auth-key", token);

                var response = await client.GetAsync($"https://financeapp-authservice.azurewebsites.net/api/get_user_for_token");
                log.Info($"status {response.StatusCode.ToString()}");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return string.Empty;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                log.Info($"content {responseContent}");
                return JObject.Parse(responseContent)["userId"].Value<string>();
            }
        }
    }
}
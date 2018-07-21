
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

namespace AccountService.Services
{
    public static class TokenService
    {
        public static async Task<string> GetUserIdForToken(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("auth-key", token);
                client.DefaultRequestHeaders.Add("x-functions-key", "gpaYFglPWlvPUCfbH/GgZTf9lFO2T48RSY4rQPH19vuQq0wiwQ7OVQ==");
                
                var response = await client.GetAsync($"https://financeapp-authservice.azurewebsites.net/api/get_user_for_token");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return string.Empty;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseContent)["userId"].Value<string>();
            }
        }
    }
}
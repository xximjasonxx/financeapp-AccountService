
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AccountService.Services
{
    public static class TokenService
    {
        public static async Task<string> GetUserIdForToken(string token)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://financeapp-authservice.azurewebsites.net/api/validate");
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.DefaultRequestHeaders.Add("x-functions-key", "gpaYFglPWlvPUCfbH/GgZTf9lFO2T48RSY4rQPH19vuQq0wiwQ7OVQ==");
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
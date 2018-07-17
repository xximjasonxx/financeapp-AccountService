
using System.Threading.Tasks;
using MongoDB.Driver;
using AccountService.Models;
using System.Net.Http;
using AccountService.Responses;
using System.Net;
using Newtonsoft.Json;

namespace AccountService.Services
{
    public static class UserService
    {
        public static async Task<CreateUserResponse> CreateUser(User newUser)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(newUser));
                client.DefaultRequestHeaders.Add("x-functions-key", "yjUjHlfwXbRAZFVBNFuhMAUOOA8MhL/XyXLxlqn7mGr/0inFn3JCyA==");
                var response = await client.PostAsync("https://financeapp-authservice.azurewebsites.net/api/create_user", content);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    // something bad happend
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CreateUserResponse>(responseContent);
            }
        }

        public static async Task<User> GetUser(string userId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://financeapp-authservice.azurewebsites.net/api/{userId}");
                client.DefaultRequestHeaders.Add("x-functions-key", "yjUjHlfwXbRAZFVBNFuhMAUOOA8MhL/XyXLxlqn7mGr/0inFn3JCyA==");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(responseContent);
            }
        }
    }
}
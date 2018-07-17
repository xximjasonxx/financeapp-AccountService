
using Newtonsoft.Json;

namespace AccountService.Responses
{
    public class CreateUserResponse
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}

using Newtonsoft.Json;

namespace AccountService.Responses
{
    public class UserResponse
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
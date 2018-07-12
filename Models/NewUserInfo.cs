
using Newtonsoft.Json;

namespace AccountService.Models
{
    public class NewUserInfo
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]          // yup - storing in plaintext cause this is a sample app
        public string Password { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zipcode")]
        public string PostalCode { get; set; }

        public string PendingApplication { get; set; }
    }
}
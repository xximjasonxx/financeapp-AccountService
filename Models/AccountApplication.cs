
using Newtonsoft.Json;

namespace AccountService.Models
{
    public class AccountApplication
    {
        public string ApplicationId { get; set; }

        public AccountStatus Status { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }

        [JsonProperty("startingBalance")]
        public decimal StartingBalance { get; set; }

        [JsonProperty("owningUserId")]
        public string OwningUserId { get; internal set; }
    }
}
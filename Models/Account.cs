
using System;
using Newtonsoft.Json;

namespace AccountService.Models
{
    public class Account
    {
        public Guid AccountId { get; set; }

        [JsonProperty("applicationId")]
        public Guid ApplicationId { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("currentBalance")]
        public decimal CurrentBalance { get; set; }
        
        [JsonIgnore]
        public AccountStatus Status { get; set; }

        [JsonProperty("status")]
        public string StatusDisplay
        {
            get { return Status.ToString(); }
        }

        [JsonProperty("owner")]
        public Guid OwnerId { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }
    }
}
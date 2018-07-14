
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace AccountService.Models
{
    public class Account
    {
        [BsonId, JsonIgnore]
        public ObjectId _Id { get; set; }

        [JsonProperty("id"), BsonIgnore]
        public string Id
        {
            get { return _Id.ToString(); }
            set
            {
                ObjectId parsedValue;
                if (ObjectId.TryParse(value, out parsedValue))
                    _Id = parsedValue;
                else
                    _Id = ObjectId.Empty;
            }
        }

        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("currentBalance")]
        public decimal CurrentBalance { get; set; }
        
        [JsonProperty("status")]
        public AccountStatus Status { get; set; }

        [JsonProperty("owner")]
        public string OwnerId { get; set; }

        [JsonProperty("type")]
        public string AccountType { get; set; }
    }
}
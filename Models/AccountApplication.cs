
using System;
using Newtonsoft.Json;

namespace AccountService.Models
{
    public class AccountApplication
    {
        public Guid ApplicationId { get; set; }

        public string AccountName { get; set; }

        public string AccountType { get; set; }
        
        public decimal StartingBalance { get; set; }
        public Guid OwningUserId { get; set; }
    }
}
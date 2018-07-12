
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using AccountService.Models;
using System.Threading.Tasks;

namespace AccountService.Functions
{
    public static class AccountService
    {
        [FunctionName("submit_application")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            string rawContents = await req.ReadAsStringAsync();
            NewUserInfo newUser = JsonConvert.DeserializeObject<NewUserInfo>(rawContents);
            AccountApplication applicationData = JsonConvert.DeserializeObject<AccountApplication>(rawContents);

            log.Info($"Username: {newUser.Username}");
            log.Info($"Account Name: {applicationData.AccountName}");

            string applicationId = Guid.NewGuid().ToString();
            // applicationData.ApplicationId = applicationId;   

            return new OkResult();
        }
    }
}

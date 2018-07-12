
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
using AccountService.Services;

namespace AccountService.Functions
{
    public static class AccountService
    {
        [FunctionName("submit_application")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            string rawContents = await req.ReadAsStringAsync();
            UserInfo newUser = JsonConvert.DeserializeObject<UserInfo>(rawContents);
            AccountApplication applicationData = JsonConvert.DeserializeObject<AccountApplication>(rawContents);

            string applicationId = Guid.NewGuid().ToString();
            applicationData.ApplicationId = applicationId; 
            newUser.PendingApplication = applicationId;

            var updatedUser = await UserService.CreateUser(newUser);

            return new OkResult();
        }
    }
}

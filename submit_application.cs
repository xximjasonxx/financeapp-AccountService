
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using AccountService.Models;

namespace AccountService.Functions
{
    public static class AccountService
    {
        [FunctionName("submit_application")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, [FromBody]NewUserInfo newUser, [FromBody]AccountApplication applicationData, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            log.Info($"Username: {newUser.Username}");
            log.Info($"Account Name: {applicationData.AccountName}");

            string applicationId = Guid.NewGuid().ToString();
            applicationData.ApplicationId = applicationId;

            return new OkResult();
        }
    }
}

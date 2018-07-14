
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
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
    public static class AccountFunctions
    {
        [FunctionName("submit_application")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            string rawContents = await req.ReadAsStringAsync();
            UserInfo newUser = JsonConvert.DeserializeObject<UserInfo>(rawContents);
            AccountApplication applicationData = JsonConvert.DeserializeObject<AccountApplication>(rawContents);

            string applicationId = Guid.NewGuid().ToString();
            applicationData.ApplicationId = applicationId;
            newUser.PendingApplication = applicationId;

            var updatedUser = await UserService.CreateUser(newUser);
            log.Info("User Created");
            applicationData.OwningUserId = updatedUser.Id;
            await AccountsService.CreateAccount(applicationData);
            log.Info("Application submitted");

            return new OkResult();
        }

        [ServiceBusAccount("ServiceBusConnectionString")]
        [FunctionName("process_application")]
        public static async Task ProcessQueueMessage([ServiceBusTrigger("accounts_to_process")] string applicationContents, TraceWriter logger)
        {
            var application = JsonConvert.DeserializeObject<AccountApplication>(applicationContents);

            // find the account matching the criteria from the application
            var account = await AccountsService.GetAccountByApplication(application);
            if (account != null)
            {
                account.Status = AccountStatus.Open;
                account.CurrentBalance = application.StartingBalance;
            }

            await AccountsService.UpdateAccountDetails(account);
        }
    }
}

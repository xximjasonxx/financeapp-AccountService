
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
        public static async Task<IActionResult> SubmitApplication([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            string rawContents = await req.ReadAsStringAsync();
            User newUser = JsonConvert.DeserializeObject<User>(rawContents);
            log.Info($"Email Address: {newUser.EmailAddress}");

            AccountApplication applicationData = JsonConvert.DeserializeObject<AccountApplication>(rawContents);

            var createdUser = await UserService.CreateUser(newUser);
            string applicationId = Guid.NewGuid().ToString();
            applicationData.ApplicationId = applicationId;
            applicationData.OwningUserId = createdUser.UserId;
            
            await AccountsService.CreateAccount(applicationData);
            log.Info("Application submitted");

            return new OkResult();
        }

        [ServiceBusAccount("ServiceBusConnectionString")]
        [FunctionName("process_application")]
        public static void ProcessApplication([ServiceBusTrigger("accounts_to_process")] string applicationContents, TraceWriter logger)
        {
            try
            {
                var application = JsonConvert.DeserializeObject<AccountApplication>(applicationContents);
                logger.Info("deserialization complete");

                // find the account matching the criteria from the application
                var account = AccountsService.GetAccountByApplication(application);
                if (account != null)
                {
                    logger.Info("found an account application");

                    account.Status = AccountStatus.Open;
                    account.CurrentBalance = application.StartingBalance;
                }

                AccountsService.UpdateAccountDetails(account);
                logger.Info("update complete");
            }
            catch (Exception aex)
            {
                logger.Error(aex.Message, aex);
            }
        }

        [FunctionName("get_accounts")]
        public static async Task<IActionResult> GetAccounts([HttpTrigger(AuthorizationLevel.User, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            return new OkObjectResult(await AccountsService.GetAccounts());
        }
    }
}


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
using AuthService.Extensions;
using System.Threading;

namespace AccountService.Functions
{
    public static class AccountFunctions
    {
        [FunctionName("submit_application")]
        public static async Task<IActionResult> SubmitApplication([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            var token = req.Headers["auth-key"].ToString().AsJwtToken();
            var user = await TokenService.GetUserIdForToken(token);
            if (user == null)
                return new NotFoundResult();

            var application = JsonConvert.DeserializeObject<AccountApplication>(await req.ReadAsStringAsync());
            application.OwningUserId = Guid.Parse(user.UserId);
            application.ApplicationId = Guid.NewGuid();

            var newAccount = await AccountsService.CreateNewAccount(application);
            await QueueService.SubmitApplicationForProcessing(application);

            // add to processing queue
            return new AcceptedResult(newAccount.AccountId.ToString(), newAccount);
        }

        [FunctionName("process_application")]
        public static async void ProcessApplication([ServiceBusTrigger("application-queue", Connection = "ServiceBusConnection")]string applicationContents, TraceWriter logger)
        {
            try
            {
                var application = JsonConvert.DeserializeObject<AccountApplication>(applicationContents);
                await Task.Run(() => {
                    Thread.Sleep(10000);
                });

                await AccountsService.ApproveAccountAsync(application);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        [FunctionName("change_account_balance")]
        public static async void ChangeAccountBalance([ServiceBusTrigger("current-balance-update-queue", Connection = "ServiceBusConnection")]string changeValueContents, TraceWriter logger)
        {
            var changeEvent = JsonConvert.DeserializeObject<AmountChangeEvent>(changeValueContents);
            await AccountsService.UpdateAccountBalanceAsync(changeEvent.TargetAccountId, changeEvent.ValueChangeAmount);
        }

        [FunctionName("get_accounts")]
        public static async Task<IActionResult> GetAccounts([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            var token = req.Headers["auth-key"].ToString().AsJwtToken();
            var user = await TokenService.GetUserIdForToken(token);
            if (user == null)
                return new NotFoundResult();

            return new OkObjectResult(await AccountsService.GetAccounts(user.UserId));
        }

        [FunctionName("get_account")]
        public static async Task<IActionResult> GetAccount([HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{id}")]HttpRequest request, string id, TraceWriter log)
        {
            var account = await AccountsService.GetAccount(id);
            if (account == null)
                return new NotFoundResult();

            return new OkObjectResult(account);
        }
    }
}

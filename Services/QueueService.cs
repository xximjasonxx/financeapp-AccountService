
using System.Text;
using System.Threading.Tasks;
using AccountService.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace AccountService.Services
{
    public static class QueueService
    {
        public static async Task SubmitApplicationForProcessing(AccountApplication application)
        {
            var sbConnectionString = "Endpoint=sb://financeapp-bus.servicebus.windows.net/;SharedAccessKeyName=SendKey;SharedAccessKey=s8Vte0tTPSiunVwK6Irau26ONeg2eaJxCtZECPVcuKc=";
            var queueName = "accounts_to_process";

            var queueClient = new QueueClient(sbConnectionString, queueName);
            var rawContents = JsonConvert.SerializeObject(application);
            var message = new Message(Encoding.UTF8.GetBytes(rawContents));
            await queueClient.SendAsync(message);
        }
    }
}
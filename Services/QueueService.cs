
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AccountService.Services
{
    public static class QueueService
    {
        public static async Task SubmitApplicationForProcessing(string applicationId)
        {
            var sbConnectionString = "Endpoint=sb://financeapp-bus.servicebus.windows.net/;SharedAccessKeyName=SendKey;SharedAccessKey=s8Vte0tTPSiunVwK6Irau26ONeg2eaJxCtZECPVcuKc=";
            var queueName = "accounts_to_process";

            var queueClient = new QueueClient(sbConnectionString, queueName);
            var message = new Message(Encoding.UTF8.GetBytes(applicationId));
            await queueClient.SendAsync(message);
        }
    }
}
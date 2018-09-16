
using System;
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
            var sbConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnection", EnvironmentVariableTarget.Process);
            var queueName = "application-queue";

            var queueClient = new QueueClient(sbConnectionString, queueName);
            var rawContents = JsonConvert.SerializeObject(application);
            var message = new Message(Encoding.UTF8.GetBytes(rawContents));
            await queueClient.SendAsync(message);
        }
    }
}
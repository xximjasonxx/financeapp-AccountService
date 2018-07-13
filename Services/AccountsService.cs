
using System.Threading.Tasks;
using AccountService.Models;
using MongoDB.Driver;

namespace AccountService.Services
{
    public static class AccountsService
    {
        public static async Task SubmitApplication(AccountApplication application)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("accounts");
            var collection = database.GetCollection<AccountApplication>("account_applications");

            application.Status = AccountStatus.Pending;
            await collection.InsertOneAsync(application);
            await QueueService.SubmitApplicationForProcessing(application.ApplicationId);
        }
    }
}
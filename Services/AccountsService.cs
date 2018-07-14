
using System;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Models;
using MongoDB.Driver;

namespace AccountService.Services
{
    public static class AccountsService
    {
        public static async Task CreateAccount(AccountApplication application)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("accounts");
            var collection = database.GetCollection<Account>("accounts");

            var account = new Account()
            {
                AccountName = application.AccountName,
                AccountType = application.AccountType,
                OwnerId = application.OwningUserId,
                Status = AccountStatus.Pending,
                ApplicationId = application.ApplicationId
            };

            await collection.InsertOneAsync(account);
            await QueueService.SubmitApplicationForProcessing(application);
        }

        public static Account GetAccountByApplication(AccountApplication application)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("accounts");
            var collection = database.GetCollection<Account>("accounts");

            var result = collection.Find(x => x.ApplicationId == application.ApplicationId &&
                x.AccountName == application.AccountName &&
                x.AccountType == application.AccountType &&
                x.OwnerId == application.OwningUserId &&
                x.Status == AccountStatus.Pending);

            return result.FirstOrDefault();
        }

        public static void UpdateAccountDetails(Account account)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("accounts");
            var collection = database.GetCollection<Account>("accounts");


            collection.ReplaceOne(x => x.Id == account.Id, account);
        }
    }
}
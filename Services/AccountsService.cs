
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Models;
using Dapper;

namespace AccountService.Services
{
    public static class AccountsService
    {
        static IDbConnection GetConnection()
        {
            var connStr = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process);
            var connection = new SqlConnection(connStr);
            connection.Open();

            return connection;
        }

        public static async Task CreateAccount(AccountApplication application)
        {
            /*var client = new MongoClient("mongodb://financeapp:alxvP9nMsU21vn6Ap0iLWnPRiKvqauHMDm0SK9jI8OwfNqIfluujL532VHqjZPg61668dt5VWAFbO2DoYpETIg==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
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
            await QueueService.SubmitApplicationForProcessing(application);*/
        }

        public static Account GetAccountByApplication(AccountApplication application)
        {
            /*var client = new MongoClient("mongodb://financeapp:alxvP9nMsU21vn6Ap0iLWnPRiKvqauHMDm0SK9jI8OwfNqIfluujL532VHqjZPg61668dt5VWAFbO2DoYpETIg==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("accounts");
            var collection = database.GetCollection<Account>("accounts");

            var result = collection.Find(x => x.ApplicationId == application.ApplicationId &&
                x.AccountName == application.AccountName &&
                x.AccountType == application.AccountType &&
                x.OwnerId == application.OwningUserId &&
                x.Status == AccountStatus.Pending);

            return result.FirstOrDefault();*/
            return null;
        }

        public static void UpdateAccountDetails(Account account)
        {
            /* var client = new MongoClient("mongodb://financeapp:alxvP9nMsU21vn6Ap0iLWnPRiKvqauHMDm0SK9jI8OwfNqIfluujL532VHqjZPg61668dt5VWAFbO2DoYpETIg==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("accounts");
            var collection = database.GetCollection<Account>("accounts");


            collection.ReplaceOne(x => x._Id == account._Id, account); */
        }

        public static async Task<IList<Account>> GetAccounts(string userId)
        {
            using (var connection = GetConnection())
            {
                var accountsResult = await connection.QueryMultipleAsync("select * from Accounts where OwnerId = @OwnerId", new { OwnerId = userId });
                return accountsResult.Read<Account>().ToList();
            }
        }

        public static async Task<Account> GetAccount(string id)
        {
            using (var connection = GetConnection())
            {
                const string sql = "select * from Accounts where AccountId = @AccountId";
                return await connection.QueryFirstOrDefaultAsync(sql, new { AccountId = id });
            }
        }
    }
}
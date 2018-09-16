
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Models;
using Dapper;
using Microsoft.Azure.WebJobs.Host;

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

        public static async Task<Account> CreateNewAccount(AccountApplication application)
        {
            var newAccount = new Account()
            {
                ApplicationId = application.ApplicationId,
                AccountId = Guid.NewGuid(),
                AccountName = application.AccountName,
                Status = AccountStatus.Pending,
                OwnerId = application.OwningUserId,
                AccountType = application.AccountType
            };

            using (var connection = GetConnection())
            {
                const string sql = "insert into Accounts(AccountId, ApplicationId, AccountName, CurrentBalance, [Status], OwnerId, AccountType) values(@AccountId, @ApplicationId, @AccountName, 0, @Status, @OwnerId, @AccountType)";
                await connection.ExecuteAsync(sql, newAccount);
            }

            return newAccount;
        }

        public static async Task<string> ApproveAccountAsync(AccountApplication application)
        {
            using (var connection = GetConnection())
            {
                const string sql = "update Accounts set [Status] = 1, CurrentBalance = @StartingBalance where ApplicationId = @ApplicationId and OwnerId = @OwningUserId";
                var rowsAffected = await connection.ExecuteAsync(sql, application);

                if (rowsAffected == 0)
                    throw new Exception("Account not found");

                return application.ApplicationId.ToString();
            }
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
                var account = await connection.QueryFirstOrDefaultAsync<Account>(sql, new { AccountId = id });

                return account;
            }
        }
    }
}
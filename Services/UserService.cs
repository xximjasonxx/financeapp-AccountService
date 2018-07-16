
using System.Threading.Tasks;
using MongoDB.Driver;
using AccountService.Models;

namespace AccountService.Services
{
    public static class UserService
    {
        public static async Task<UserInfo> CreateUser(UserInfo newUser)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<UserInfo>("users");

            await collection.InsertOneAsync(newUser);

            return newUser;
        }

        public static async Task<UserInfo> GetUser(string userId)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<UserInfo>("users");

            var result = await collection.FindAsync(x => x.Id == userId);
            return result.FirstOrDefault();
        }
    }
}
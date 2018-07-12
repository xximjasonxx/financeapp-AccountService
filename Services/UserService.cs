
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
            var database = client.GetDatabase("user_info");
            var collection = database.GetCollection<UserInfo>("user_data");

            await collection.InsertOneAsync(newUser);

            return newUser;
        }
    }
}
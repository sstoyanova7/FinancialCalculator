using Dapper;
using FinancialCalculator.BL.Configuration.Database;
using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinancialCalculator.BL.Services
{
    class UserDataService : IUserDataService
    {
        private readonly IDatabaseConnectionFactory _database;

        public UserDataService(IDatabaseConnectionFactory database)
        {
            _database = database;
        }
        public async Task<UserRequestModel> getUserById(long id)
        {
            using var conn = await _database.CreateConnectionAsync();
            var parameters = new { Id = id };
            var result = conn.QueryFirstOrDefault<UserRequestModel>
               ("SELECT * FROM [User] WHERE Id = @Id", parameters);
            return result;
        }

        public async Task<List<UserRequestModel>> getAllUsers()
        {
            using var conn = await _database.CreateConnectionAsync();
            var result = conn.Query<UserRequestModel>
               ("SELECT * FROM [User]").AsList();
            return result;
        }

        public async void insertUser(UserCreateRequestModel user)
        {
            using var conn = await _database.CreateConnectionAsync();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);

            var parameters = new
            {
                Name = user.name,
                Email = user.email,
                Password = user.password,
            };

            await conn.ExecuteAsync("INSERT INTO [USER](Name, Email, Password) VALUES (@Name, @Email, @Password)", parameters);
        }

        public async void deleteUserById(long id)
        {
            using var conn = await _database.CreateConnectionAsync();
            var parameters = new { Id = id };

            await conn.ExecuteAsync("DELETE FROM [USER] WHERE Id=@Id", parameters);
        }

        private UserModel fromDtoToUser(UserCreateRequestModel userRequestModel)
        {
            UserModel userModel = new UserModel();
            userRequestModel.name = userModel.name;
            userRequestModel.email = userModel.email;
            userRequestModel.password = userModel.password;
            return userModel;
        }
    }
}

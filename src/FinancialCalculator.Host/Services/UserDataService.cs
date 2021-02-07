using Dapper;
using FinancialCalculator.DAL.Configuration.Database;
using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinancialCalculator.Host.Services
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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var parameters = new
            {
                Username = user.Username,
                Email = user.Email,
                Password = hashedPassword,
            };

            await conn.ExecuteAsync("INSERT INTO [USER](Username, Email, Password) VALUES (@Username, @Email, @Password)", parameters);
        }

        public async void deleteUserById(long id)
        {
            using var conn = await _database.CreateConnectionAsync();
            var parameters = new { Id = id };

            await conn.ExecuteAsync("DELETE FROM [USER] WHERE Id=@Id", parameters);
        }

        public async Task<UserModel> getFullUserByName(string name)
        {
            // add handling for not existing user
            using var conn = await _database.CreateConnectionAsync();
            var parameters = new { Username = name };
            var result = conn.QueryFirstOrDefault<UserModel>
               ("SELECT * FROM [User] WHERE Username = @Username", parameters);
            return result;
        }
        public async Task<bool> isUserExisting(string name, string email)
        {
            using var conn = await _database.CreateConnectionAsync();
            var parameters = new { Username = name, Email = email };
            var result = conn.QueryFirstOrDefault<UserModel>
               ("SELECT * FROM [User] WHERE Username = @Username and Email = @Email", parameters);
            return result != null ? true : false;
        }

        public bool isUserPasswordCorrect(string hashedPassword, string enteredPassowrd)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassowrd, hashedPassword);            
        }

        private UserModel fromDtoToUser(UserCreateRequestModel userRequestModel)
        {
            UserModel userModel = new UserModel();
            userRequestModel.Username = userModel.Username;
            userRequestModel.Email = userModel.Email;
            userRequestModel.Password = userModel.Password;
            return userModel;
        }
    }
}

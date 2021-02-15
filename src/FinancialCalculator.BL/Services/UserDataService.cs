namespace FinancialCalculator.BL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using BCrypt.Net;
    using Dapper;
    using FinancialCalculator.BL.Configuration.Database;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using FinancialCalculator.Services;

    class UserDataService : IUserDataService
    {
        private readonly int _minCharValidationUsername = 4;
        private readonly int _maxCharValidationUsername = 40;
        private readonly int _minCharValidationPassword = 6;
        private readonly int _maxCharValidationPassword = 40;
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
        public async Task<UserRequestModel> getUserByName(string name)
        {
            using var conn = await _database.CreateConnectionAsync();
            var parameters = new { Username = name };
            var result = conn.QueryFirstOrDefault<UserRequestModel>
               ("SELECT * FROM [User] WHERE Username = @Username", parameters);
            return result;
        }

        public async Task<List<UserRequestModel>> getAllUsers()
        {
            using var conn = await _database.CreateConnectionAsync();
            var result = conn.Query<UserRequestModel>
               ("SELECT * FROM [User]").AsList();
            return result;
        }

        public async Task<int> InsertUser(UserCreateRequestModel user)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Match match = regex.Match(user.Email);
            if (!match.Success)
            {
                throw new Exception("Email must have '@' and after that <string>.<string>, e.g. not_existing_email@example.com");
            }
            validateString(user.Username, "Username must between " + _minCharValidationUsername + " and " + _maxCharValidationUsername + " symbols!");
            validateString(user.Username, "Password must between " + _minCharValidationPassword + " and " + _maxCharValidationPassword + " symbols!");
            if (!user.Password.Contains(user.Password2))
            {
                throw new Exception("Entered Password and Confirm password do not match");
            }
            if (await IsUserExistingByName(user.Username))
            {
                throw new Exception("User with username: " + user.Username  + " already exist!");
            }

            using var conn = await _database.CreateConnectionAsync();
            string hashedPassword = BCrypt.HashPassword(user.Password);

            var parameters = new
            {
                Username = user.Username,
                Email = user.Email,
                Password = hashedPassword,
            };
            
            return await conn.ExecuteAsync("INSERT INTO [USER](Username, Email, Password) VALUES (@Username, @Email, @Password)", parameters);
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
        public async Task<bool> IsUserExistingByName(string username)
        {
            using var conn = await _database.CreateConnectionAsync();
            var parameters = new { Username = username };
            var result = conn.QueryFirstOrDefault<UserModel>
               ("SELECT * FROM [User] WHERE Username = @Username", parameters);
            return result != null ? true : false;
        }
        public bool isUserPasswordCorrect(string hashedPassword, string enteredPassowrd)
        {
            return BCrypt.Verify(enteredPassowrd, hashedPassword);            
        }
        private UserModel fromDtoToUser(UserCreateRequestModel userRequestModel)
        {
            UserModel userModel = new UserModel();
            userRequestModel.Username = userModel.Username;
            userRequestModel.Email = userModel.Email;
            userRequestModel.Password = userModel.Password;
            return userModel;
        }
        private void validateString(string value, string errorMessage)
        {
            if (value.Length < 4 || value.Length > 40)
            {
                throw new Exception(errorMessage);
            }
        }
    }
}

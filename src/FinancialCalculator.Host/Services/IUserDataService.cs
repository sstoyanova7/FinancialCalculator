﻿namespace FinancialCalculator.Host.Services
{
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserDataService
    {
        Task<UserRequestModel> getUserById(long id);

        void insertUser(UserCreateRequestModel user);

        void deleteUserById(long id);

        Task<List<UserRequestModel>> getAllUsers();

        Task<UserModel> getFullUserByName(string name);

        bool isUserPasswordCorrect(string hashedPassword, string enteredPassowrd);

        Task<bool> isUserExisting(string name, string email);
    }
}

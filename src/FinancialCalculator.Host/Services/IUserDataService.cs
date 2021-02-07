using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinancialCalculator.Host.Services
{
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

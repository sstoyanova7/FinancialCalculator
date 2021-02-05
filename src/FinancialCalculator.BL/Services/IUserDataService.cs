using FinancialCalculator.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinancialCalculator.BL.Services
{
    public interface IUserDataService
    {
        Task<UserRequestModel> getUserById(long id);

        void insertUser(UserCreateRequestModel user);

        void deleteUserById(long id);

        Task<List<UserRequestModel>> getAllUsers();
    }
}

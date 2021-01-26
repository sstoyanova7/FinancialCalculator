using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FinancialCalculator.Models.ResponseModels;
using FinancialCalculator.Models.RequestModels;

namespace FinancialCalculator.BL.Services.DatabaseServices
{
    public interface IUserDataService
    {

        Task<UserRequestModel> getUserById(long id);

        void insertUser(UserCreateRequestModel user);

        void deleteUserById(long id);

        Task<List<UserRequestModel>> getAllUsers();
    }
}

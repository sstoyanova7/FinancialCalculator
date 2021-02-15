using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using System.Threading.Tasks;

namespace FinancialCalculator.Services
{
    public interface IJWTService
    {
        string GenerateJSONWebToken(UserLoginRequestModel userInfo);
        Task<UserRequestModel> decodeJWTAsync(string jwt);
    }
}

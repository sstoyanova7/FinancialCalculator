namespace FinancialCalculator.Host.Services
{
    using FinancialCalculator.Models.RequestModels;

    public interface IJWTService
    {
        string GenerateJSONWebToken(UserLoginRequestModel userInfo);
        void decodeJWT(string jwt);
    }
}

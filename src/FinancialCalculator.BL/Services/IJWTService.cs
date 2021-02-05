using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialCalculator.BL.Services
{
    public interface IJWTService
    {
        string GenerateJSONWebToken(UserLoginRequestModel userInfo);
        void decodeJWT(string jwt);
    }
}

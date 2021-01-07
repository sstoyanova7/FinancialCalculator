using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using Microsoft.Extensions.Logging;

namespace FinancialCalculator.Services
{
    public interface ICalculatorService
    {
        NewLoanResponseModel CalculateNewLoan(NewLoanRequestModel requestModel);
        LeasingLoanResponseModel CalculateLeasingLoan(LeasingLoanRequestModel requestModel);
    }
}

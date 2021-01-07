using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;

namespace FinancialCalculator.Services
{
    public interface ICalculatorService
    {
        NewLoanResponseModel CalculateNewLoan(NewLoanRequestModel requestModel);
    }
}

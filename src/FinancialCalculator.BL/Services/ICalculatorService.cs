namespace FinancialCalculator.Services
{
    using Models.RequestModels;
    using Models.ResponseModels;

    public interface ICalculatorService
    {
        NewLoanResponseModel CalculateNewLoan(NewLoanRequestModel requestModel);
        LeasingLoanResponseModel CalculateLeasingLoan(LeasingLoanRequestModel requestModel);
    }
}

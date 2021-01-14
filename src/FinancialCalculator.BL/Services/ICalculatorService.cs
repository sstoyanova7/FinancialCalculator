namespace FinancialCalculator.Services
{
    using Models.RequestModels;
    using Models.ResponseModels;

    public interface ICalculatorService
    {
        NewLoanResponseModel CalculateNewLoan(NewLoanRequestModel requestModel);
        RefinancingLoanResponseModel CalculateRefinancingLoan(RefinancingLoanRequestModel requestModel);
        LeasingLoanResponseModel CalculateLeasingLoan(LeasingLoanRequestModel requestModel);
    }
}

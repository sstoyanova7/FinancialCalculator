namespace FinancialCalculator.Services
{
    using FinancialCalculator.BL.Utilities;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using System;
    using Serilog;
    using System.Net;
    using FinancialCalculator.BL.Validation;

    public class LeasingLoanCalculatorService : ICalculatorService<LeasingLoanResponseModel, LeasingLoanRequestModel>
    {

        private readonly ILogger _logger;
        private IValidator<LeasingLoanRequestModel> _leasingLoanValidator;

        public LeasingLoanCalculatorService(ILogger logger,
            IValidator<LeasingLoanRequestModel> leasingLoanValidator)
        {
            _logger = logger.ForContext<LeasingLoanCalculatorService>();
            _leasingLoanValidator = leasingLoanValidator;
        }

        public LeasingLoanResponseModel Calculate(LeasingLoanRequestModel requestModel)
        {
            var validated = _leasingLoanValidator.Validate(requestModel);
            if (!validated.IsValid)
            {
                _logger.Error($"Leasing Loan BadRequest! {validated}");

                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.BadRequest,
                    ErrorMessage = validated.ToString()
                };
            }

            //TO DO: AnnualPercentCost how to calculate ?
            try
            {
                var totalFees = CalcHelpers.GetFeeCost(requestModel.StartingFee, requestModel.ProductPrice);
                var totalCost = totalFees + requestModel.StartingInstallment + requestModel.Period * requestModel.MonthlyInstallment;

                if (totalCost < requestModel.ProductPrice)
                {
                    return new LeasingLoanResponseModel
                    {
                        Status = HttpStatusCode.BadRequest,
                        ErrorMessage = "You cannot have a leasing loan with these parameters."
                    };
                }
                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.OK,
                    //AnnualPercentCost
                    TotalCost = totalCost,
                    TotalFees = totalFees
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occured while trying to calculate leasing loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.InternalServerError,
                    ErrorMessage = "Something went wrong. Please contact our support team."
                };
            }
        }
    }
}

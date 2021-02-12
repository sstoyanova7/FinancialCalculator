namespace FinancialCalculator.Services
{
    using FinancialCalculator.BL.Utilities;
    using FinancialCalculator.Models.Enums;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Serilog;
    using System.Net;
    using FinancialCalculator.BL.Validation;

    public class NewLoanCalculatorService : ICalculatorService<NewLoanResponseModel, NewLoanRequestModel>
    {

        private readonly ILogger _logger;
        private IValidator<NewLoanRequestModel> _newLoanValidator;


        public NewLoanCalculatorService(ILogger logger,
            IValidator<NewLoanRequestModel> newLoanValidator)
        {
            _logger = logger.ForContext<NewLoanCalculatorService>();
            _newLoanValidator = newLoanValidator;
        }

        public NewLoanResponseModel Calculate (NewLoanRequestModel requestModel)
        {
            var validated = _newLoanValidator.Validate(requestModel);
            if (!validated.IsValid)
            {
                _logger.Error($"New Loan BadRequest! {validated}");

                return new NewLoanResponseModel
                {
                    Status = HttpStatusCode.BadRequest,
                    ErrorMessage = validated.ToString()
                };
            }

            try
            {

                var plan = requestModel.InstallmentType == Installments.AnnuityInstallment
                        ? CalcHelpers.GetAnuityPlan(requestModel).ToList()
                        : CalcHelpers.GetDecreasingPlan(requestModel).ToList();

                var totalFeesCost = plan.Sum(x => x.Fees);
                var totalMonthlyInstallmentsCost = plan.Sum(x => x.MonthlyInstallment);
                var totalInterestCost = plan.Sum(x => x.InterestInstallment);
                return new NewLoanResponseModel
                {
                    Status = HttpStatusCode.OK,
                    //AnnualPercentCost = CalcHelpers.CalculateAPR(totalFeesCost, totalInterestCost, requestModel.LoanAmount, requestModel.Period),
                    AnnualPercentCost = 0,
                    TotalCost = totalMonthlyInstallmentsCost + totalFeesCost,
                    FeesCost = totalFeesCost,
                    InterestsCost = totalInterestCost,
                    InstallmentsCost = totalMonthlyInstallmentsCost,
                    RepaymentPlan = plan
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occured while trying to calculate new loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new NewLoanResponseModel
                {
                    Status = HttpStatusCode.InternalServerError,
                    ErrorMessage = "Something went wrong. Please contact our support team."
                };
            }
        }       
    }
}

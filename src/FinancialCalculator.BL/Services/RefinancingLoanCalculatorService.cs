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

    public class RefinancingLoanCalculatorService : ICalculatorService<RefinancingLoanResponseModel, RefinancingLoanRequestModel>
    {

        private readonly ILogger _logger;
        private IValidator<RefinancingLoanRequestModel> _refinancingLoanValidator;
        private ICalculatorService<NewLoanResponseModel, NewLoanRequestModel> _newLoanService;

        public RefinancingLoanCalculatorService(ILogger logger,
            IValidator<RefinancingLoanRequestModel> refinancingLoanValidator,
            ICalculatorService<NewLoanResponseModel, NewLoanRequestModel> newLoanService)
        {
            _logger = logger.ForContext<RefinancingLoanCalculatorService>();
            _refinancingLoanValidator = refinancingLoanValidator;
            _newLoanService = newLoanService;
        }
       
        public RefinancingLoanResponseModel Calculate(RefinancingLoanRequestModel requestModel)
        {
            var validated = _refinancingLoanValidator.Validate(requestModel);
            if (!validated.IsValid)
            {
                _logger.Error($"Refinancing Loan BadRequest! {validated}");

                return new RefinancingLoanResponseModel
                {
                    Status = HttpStatusCode.BadRequest,
                    ErrorMessage = validated.ToString()
                };
            }

            try
            {
                var currentLoanCalculated = _newLoanService.Calculate(
                    new NewLoanRequestModel
                    {
                        LoanAmount = requestModel.LoanAmount,
                        Period = requestModel.Period,
                        Interest = requestModel.Interest,
                        InstallmentType = Installments.AnnuityInstallment
                    });

                var periodLeft = requestModel.Period - requestModel.CountOfPaidInstallments;
                var monthlyInstallmentCurrentLoan = currentLoanCalculated.RepaymentPlan.First(x => x.Id == 1).MonthlyInstallment;

                var moneyLeftToBePaid = requestModel.LoanAmount;

                for (var i = 1; i <= requestModel.CountOfPaidInstallments; i++)
                {
                    moneyLeftToBePaid -= currentLoanCalculated.RepaymentPlan.First(x => x.Id == i).PrincipalInstallment;
                }

                var earlyInstallmentsFeeInCurrency = CalcHelpers.GetFeeCost(requestModel.EarlyInstallmentsFee, moneyLeftToBePaid);

                var newLoanCalculated = _newLoanService.Calculate(
                    new NewLoanRequestModel
                    {
                        LoanAmount = Math.Floor(moneyLeftToBePaid),
                        Period = periodLeft,
                        Interest = requestModel.NewInterest,
                        InstallmentType = Installments.AnnuityInstallment,
                        Fees = new List<FeeModel>
                        {
                            new FeeModel
                            {
                                Type = FeeType.StartingApplicationFee,
                                Value = requestModel.StartingFeesCurrency,
                                ValueType = FeeValueType.Currency
                            },
                            new FeeModel
                            {
                                Type = FeeType.OtherStartingFees,
                                Value = requestModel.StartingFeesPercent,
                                ValueType = FeeValueType.Percent
                            }
                        }
                    });

                var monthlyInstallmentNewLoan = newLoanCalculated.RepaymentPlan.First(x => x.Id == 1).MonthlyInstallment;
                var totalCostNewLoan = periodLeft * monthlyInstallmentNewLoan + earlyInstallmentsFeeInCurrency
                        + requestModel.StartingFeesCurrency + requestModel.StartingFeesPercent
                        + CalcHelpers.GetFeeCost(requestModel.StartingFeesPercent, moneyLeftToBePaid);
                return new RefinancingLoanResponseModel
                {
                    Status = HttpStatusCode.OK,
                    CurrentLoan = new RefinancingLoanHelperModel
                    {
                        Interest = requestModel.Interest,
                        Period = requestModel.Period,
                        EarlyInstallmentsFee = earlyInstallmentsFeeInCurrency,
                        MonthlyInstallment = monthlyInstallmentCurrentLoan,
                        Total = periodLeft * monthlyInstallmentCurrentLoan
                    },
                    NewLoan = new RefinancingLoanHelperModel
                    {
                        Interest = requestModel.NewInterest,
                        Period = periodLeft,
                        MonthlyInstallment = monthlyInstallmentNewLoan,
                        Total = totalCostNewLoan
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occured while trying to calculate new loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new RefinancingLoanResponseModel
                {
                    Status = HttpStatusCode.InternalServerError,
                    ErrorMessage = "Something went wrong. Please contact our support team."
                };
            }
        }
    }
}

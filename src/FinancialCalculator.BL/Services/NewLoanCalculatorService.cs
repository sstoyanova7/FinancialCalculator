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

            //TO DO: PromoPeriod % Promoratе
            //TO DO: calculate AnnualPercentCost ?
            try
            {

                var plan = requestModel.InstallmentType == Installments.AnnuityInstallment
                        ? GetAnuityPlan(requestModel).ToList()
                        : GetDecreasingPlan(requestModel).ToList();

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

        private IEnumerable<InstallmentForRepaymentPlanModel> GetDecreasingPlan(NewLoanRequestModel requestModel)
        {
            var actualPeriod = requestModel.Period - requestModel.GracePeriod;
            var principalInstallment = Math.Round(requestModel.LoanAmount / actualPeriod, 2);
            var lastPrincipalInstallment = requestModel.LoanAmount - (actualPeriod - 1) * principalInstallment;
            var installments = new Dictionary<int, InstallmentForRepaymentPlanModel>();
            var startingFees = CalcHelpers.CalculateStartingFeesCost(requestModel.LoanAmount, requestModel.Fees);

            var zero = new InstallmentForRepaymentPlanModel
            {
                Id = 0,
                Date = DateTime.UtcNow.Date,
                MonthlyInstallment = 0,
                PrincipalInstallment = 0,
                InterestInstallment = 0,
                PrincipalBalance = requestModel.LoanAmount,
                Fees = startingFees,
                CashFlow = requestModel.LoanAmount - startingFees
            };

            installments.Add(0, zero);

            for (var i = 1; i <= requestModel.Period; i++)
            {
                var interest = i <= requestModel.PromoPeriod ? requestModel.PromoInterest : requestModel.Interest;
                decimal currentPrincipalBalance;
                decimal interestInstallment;
                decimal currentPrincipalInstallment;
                decimal monthlyInstallment;

                if (i <= requestModel.GracePeriod)
                {
                    currentPrincipalBalance = requestModel.LoanAmount;
                    interestInstallment = CalcHelpers.CalculateInterestInstallment(interest, currentPrincipalBalance);
                    currentPrincipalInstallment = 0;
                    monthlyInstallment = interestInstallment;
                }
                else
                { 
                    currentPrincipalBalance = CalcHelpers.CalculatePrincipalBalance(installments[i - 1]);
                    interestInstallment = CalcHelpers.CalculateInterestInstallment(interest, currentPrincipalBalance);
                    currentPrincipalInstallment = i == requestModel.Period ? lastPrincipalInstallment : principalInstallment;
                    monthlyInstallment = interestInstallment + currentPrincipalInstallment;
                }

                var fees = CalcHelpers.CalculateFeesCost(i, currentPrincipalBalance, requestModel.Fees);

                installments.Add(i, new InstallmentForRepaymentPlanModel
                {
                    Id = i,
                    Date = DateTime.UtcNow.Date.AddDays(i),
                    MonthlyInstallment = monthlyInstallment,
                    PrincipalInstallment = currentPrincipalInstallment,
                    InterestInstallment = interestInstallment,
                    PrincipalBalance = currentPrincipalBalance,
                    Fees = fees,
                    CashFlow = -monthlyInstallment - fees
                });
            }

            return installments.Select(x => x.Value);
        }

        private IEnumerable<InstallmentForRepaymentPlanModel> GetAnuityPlan(NewLoanRequestModel requestModel)
        {
            var actualPeriod = requestModel.Period - requestModel.GracePeriod;
            var pmt = CalcHelpers.CalculatePMT(requestModel.Interest, actualPeriod, requestModel.LoanAmount);

            var installments = new Dictionary<int, InstallmentForRepaymentPlanModel>();
            var startingFees = CalcHelpers.CalculateStartingFeesCost(requestModel.LoanAmount, requestModel.Fees);
            var zero = new InstallmentForRepaymentPlanModel
            {
                Id = 0,
                Date = DateTime.UtcNow.Date,
                MonthlyInstallment = 0,
                PrincipalInstallment = 0,
                InterestInstallment = 0,
                PrincipalBalance = requestModel.LoanAmount,
                Fees = startingFees,
                CashFlow = requestModel.LoanAmount - startingFees
            };

            installments.Add(0, zero);

            for (var i = 1; i <= requestModel.Period; i++)
            {
                decimal currentPrincipalBalance;
                decimal interestInstallment;
                decimal fees;
                decimal principalInstallment;
                decimal monthlyInstallment;

                if (i <= requestModel.GracePeriod)
                {
                    currentPrincipalBalance = requestModel.LoanAmount;
                    interestInstallment = CalcHelpers.CalculateInterestInstallment(requestModel.Interest, currentPrincipalBalance);
                    fees = CalcHelpers.CalculateFeesCost(i, currentPrincipalBalance, requestModel.Fees);
                    principalInstallment = 0;
                    monthlyInstallment = interestInstallment;
                }
                else
                {
                    currentPrincipalBalance = CalcHelpers.CalculatePrincipalBalance(installments[i - 1]);
                    interestInstallment = CalcHelpers.CalculateInterestInstallment(requestModel.Interest, currentPrincipalBalance);
                    fees = CalcHelpers.CalculateFeesCost(i, currentPrincipalBalance, requestModel.Fees);
                    principalInstallment = i == requestModel.Period ? requestModel.LoanAmount - installments.Sum(x => x.Value.PrincipalInstallment) : pmt - interestInstallment;
                    monthlyInstallment = i == requestModel.Period ? principalInstallment + interestInstallment : pmt;
                } 
                
                installments.Add(i, new InstallmentForRepaymentPlanModel
                {
                    Id = i,
                    Date = DateTime.UtcNow.Date.AddDays(i),
                    MonthlyInstallment = monthlyInstallment,
                    PrincipalInstallment = principalInstallment,
                    InterestInstallment = interestInstallment,
                    PrincipalBalance = currentPrincipalBalance,
                    Fees = fees,
                    CashFlow = -monthlyInstallment - fees
                });
            }

            return installments.Select(x => x.Value);
        }
    }
}

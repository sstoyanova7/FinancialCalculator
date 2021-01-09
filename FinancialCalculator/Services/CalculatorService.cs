using FinancialCalculator.Enums;
using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using FinancialCalculator.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FinancialCalculator.Services
{
    public class CalculatorService : ICalculatorService
    {

        private readonly ILogger<CalculatorService> _logger;

        
        public CalculatorService(ILogger<CalculatorService> logger)
        {
            _logger = logger;
        }

        public NewLoanResponseModel CalculateNewLoan(NewLoanRequestModel requestModel)
        {

            //TO DO: PromoPeriod % Promorate
            //TO DO: GracePeriod
            //TO DO: think of validations
            //TO DO: add exception handling
            //TO DO: check logs format
            //TO DO: calculate AnnualPercentCost ?!
            //TO DO: why the fuck the last monthly installment is different
            try
            {

                var plan = requestModel.InstallmentType == Installments.AnnuityInstallment
                        ? GetAnuityPlan(requestModel).ToList()
                        : GetDecreasingPlan(requestModel).ToList();

                var totalFeesCost = plan.Sum(x => x.Fees);
                var totalMonthlyInstallmentsCost = plan.Sum(x => x.MonthlyInstallment);

                return new NewLoanResponseModel
                {
                    Status = System.Net.HttpStatusCode.OK,
                    AnnualPercentCost = 0, //how the fuck is this calculated
                    TotalCost = totalMonthlyInstallmentsCost + totalFeesCost,
                    FeesCost = totalFeesCost,
                    InterestsCost = plan.Sum(x => x.InterestInstallment),
                    InstallmentsCost = totalMonthlyInstallmentsCost,
                    RepaymentPlan = plan
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to calculate new loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new NewLoanResponseModel
                {
                    Status = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }

        public LeasingLoanResponseModel CalculateLeasingLoan(LeasingLoanRequestModel requestModel)
        {
            //TO DO: AnnualPercentCost how to calculate ?
            try
            {
                var startingFeeCost = CalcHelpers.GetFeeCost(requestModel.StartingFee, requestModel.ProductPrice);
                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.OK,
                    //AnnualPercentCost { get; set; } //in percent
                    TotalCost = startingFeeCost + requestModel.StartingInstallment + requestModel.Period * requestModel.MonthlyInstallment,
                    TotalFees = startingFeeCost
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to calculate leasing loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        //TO DO: Decreasing and Annuity Plan -> check diffs and maybe combine in one method

        private IEnumerable<InstallmentForRepaymentPlanModel>  GetDecreasingPlan(NewLoanRequestModel requestModel)
        {
            var principalInstallment = Math.Round(requestModel.LoanAmount / requestModel.Period, 2);
            var lastPrincipalInstallment = requestModel.LoanAmount - (requestModel.Period - 1) * principalInstallment;

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
                var currentPrincipalBalance = CalcHelpers.CalculatePrincipalBalance(installments[i - 1]);
                var interestInstallment = CalcHelpers.CalculateInterestInstallment(requestModel.Interest, currentPrincipalBalance);
                var currentPrincipalInstallment = i == requestModel.Period ? lastPrincipalInstallment : principalInstallment;
                var monthlyInstallment = interestInstallment + currentPrincipalInstallment;
                var fees = CalcHelpers.CalculateFeesCost(i, currentPrincipalBalance, requestModel.Fees);

                installments.TryAdd(i, new InstallmentForRepaymentPlanModel
                {
                    Id = i,
                    Date = DateTime.UtcNow.Date.AddDays(i),
                    MonthlyInstallment = monthlyInstallment,
                    PrincipalInstallment = currentPrincipalInstallment,
                    InterestInstallment = interestInstallment,
                    PrincipalBalance = currentPrincipalBalance,
                    Fees = fees,
                    CashFlow = - monthlyInstallment - fees
                });
            }

            return installments.Select(x => x.Value);

            throw new NotImplementedException();
        }

        private IEnumerable<InstallmentForRepaymentPlanModel> GetAnuityPlan(NewLoanRequestModel requestModel)
        {
            var pmt = CalcHelpers.CalculatePMT(requestModel.Interest, requestModel.Period, requestModel.LoanAmount);

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
                var currentPrincipalBalance = CalcHelpers.CalculatePrincipalBalance(installments[i - 1]);
                var interestInstallment = CalcHelpers.CalculateInterestInstallment(requestModel.Interest, currentPrincipalBalance);
                var fees = CalcHelpers.CalculateFeesCost(i, currentPrincipalBalance, requestModel.Fees);
                var principalInstallment = i == requestModel.Period ? requestModel.LoanAmount - installments.Sum(x => x.Value.PrincipalInstallment) : pmt - interestInstallment;
                var monthlyInstallment = i == requestModel.Period ? principalInstallment + interestInstallment : pmt;
                installments.TryAdd(i, new InstallmentForRepaymentPlanModel
                {
                    Id = i,
                    Date = DateTime.UtcNow.Date.AddDays(i),
                    MonthlyInstallment = monthlyInstallment,
                    PrincipalInstallment = principalInstallment,
                    InterestInstallment = interestInstallment,
                    PrincipalBalance = currentPrincipalBalance,
                    Fees = fees,
                    CashFlow = - monthlyInstallment - fees
                });
            }

            return installments.Select(x => x.Value);
        }
    }
}

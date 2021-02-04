﻿namespace FinancialCalculator.Services
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

            //TO DO: PromoPeriod % Promorate
            //TO DO: GracePeriod
            //TO DO: think of other validations
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
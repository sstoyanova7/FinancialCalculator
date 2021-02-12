namespace FinancialCalculator.BL.Utilities
{
    using Models.Enums;
    using Models.RequestModels;
    using Models.ResponseModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class CalcHelpers
    {
        public static decimal CalculatePMT(decimal interestRate, int period, decimal loanAmount)
        {
            var rate = interestRate / 100 / 12;
            var denominator = (decimal)Math.Pow((1 + (double)rate), period) - 1;
            return Math.Round(((rate + (rate / denominator)) * loanAmount), 2);
        }

        //Annual Percentage Rate 
        public static decimal CalculateAPR(decimal fees, decimal interest, decimal principal, decimal period)
        {
            return Math.Round(((fees + interest) / principal / period) * 365 * 100, 2);
        }

        public static decimal CalculateStartingFeesCost(decimal loanAmount, IEnumerable<FeeModel> fees)
        {
            //Get the sum of all starting fees
            return UtilsHelpers.GetStartingFees(fees).Select(x => GetFeeCost(x, loanAmount)).Sum();
        }

        public static decimal CalculateFeesCost(int monthNumber, decimal principalBalance, IEnumerable<FeeModel> fees)
        {
            var feeCost = 0m;
            //Add the sum of all annual fees
            if(monthNumber != 1 && monthNumber % 12 == 1)
            {
                feeCost += UtilsHelpers.GetAnnualFees(fees).Select(x => GetFeeCost(x, principalBalance)).Sum();
            }

            //Add the sum of all monthly fees
            feeCost += UtilsHelpers.GetMonthlyFees(fees).Select(x => GetFeeCost(x, principalBalance)).Sum();

            return feeCost;
        }

        public static decimal CalculatePrincipalBalance(InstallmentForRepaymentPlanModel previousInstallment)
        {
            return previousInstallment.PrincipalBalance - previousInstallment.PrincipalInstallment;
        }

        public static decimal CalculateInterestInstallment(decimal interest, decimal balance)
        {
            var rate = interest / 100 / 12;
            return Math.Round(rate * balance, 2);
        }

        public static decimal GetFeeCost(FeeModel fee, decimal loanAmount)
        {
            if(fee == null)
            {
                return 0;
            }
            return fee.ValueType == FeeValueType.Currency ? fee.Value : GetFeeCost(fee.Value, loanAmount);
        }

        public static decimal GetFeeCost(decimal feePercent, decimal loanAmount)
        {
            return Math.Round(loanAmount * feePercent / 100, 2);
        }

        public static IEnumerable<InstallmentForRepaymentPlanModel> GetDecreasingPlan(NewLoanRequestModel requestModel)
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

                if (i <= requestModel.GracePeriod)
                {
                    currentPrincipalBalance = requestModel.LoanAmount;
                    interestInstallment = CalcHelpers.CalculateInterestInstallment(interest, currentPrincipalBalance);
                    currentPrincipalInstallment = 0;
                }
                else
                {
                    currentPrincipalBalance = CalcHelpers.CalculatePrincipalBalance(installments[i - 1]);
                    interestInstallment = CalcHelpers.CalculateInterestInstallment(interest, currentPrincipalBalance);
                    currentPrincipalInstallment = i == requestModel.Period ? lastPrincipalInstallment : principalInstallment;
                }

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

        public static IEnumerable<InstallmentForRepaymentPlanModel> GetAnuityPlan(NewLoanRequestModel requestModel)
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
                var interest = i <= requestModel.PromoPeriod ? requestModel.PromoInterest : requestModel.Interest;
                decimal currentPrincipalBalance;
                decimal interestInstallment;
                decimal principalInstallment;
                decimal monthlyInstallment;

                if (i <= requestModel.GracePeriod)
                {
                    currentPrincipalBalance = requestModel.LoanAmount;
                    interestInstallment = CalcHelpers.CalculateInterestInstallment(interest, currentPrincipalBalance);
                    principalInstallment = 0;
                    monthlyInstallment = interestInstallment;
                }
                else
                {
                    currentPrincipalBalance = CalcHelpers.CalculatePrincipalBalance(installments[i - 1]);
                    interestInstallment = CalcHelpers.CalculateInterestInstallment(interest, currentPrincipalBalance);

                    principalInstallment = i == requestModel.Period ? requestModel.LoanAmount - installments.Sum(x => x.Value.PrincipalInstallment) : pmt - interestInstallment;
                    monthlyInstallment = i == requestModel.Period ? principalInstallment + interestInstallment : pmt;
                }

                var fees = CalcHelpers.CalculateFeesCost(i, currentPrincipalBalance, requestModel.Fees);

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

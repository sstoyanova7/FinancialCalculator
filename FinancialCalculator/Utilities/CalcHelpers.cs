using FinancialCalculator.Enums;
using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialCalculator.Utilities
{
    public static class CalcHelpers
    {
        public static decimal CalculatePMT(decimal interestRate, int period, decimal loanAmount)
        {
            var rate = interestRate / 100 / 12;
            var denominator = (decimal)Math.Pow((1 + (double)rate), period) - 1;
            return Math.Round(((rate + (rate / denominator)) * loanAmount), 2);
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
            return fee.ValueType == FeeValueType.Currency ? fee.Value : Math.Round(loanAmount * fee.Value / 100, 2);
        }
    }
}

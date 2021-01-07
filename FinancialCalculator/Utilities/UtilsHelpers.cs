using FinancialCalculator.Enums;
using FinancialCalculator.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialCalculator.Utilities
{
    public static class UtilsHelpers
    {
        public static IEnumerable<FeeModel> GetStartingFees(IEnumerable<FeeModel> allFees)
        {
            return allFees.Where(x => x.Type == FeeType.StartingApplicationFee
                            || x.Type == FeeType.StartingProcessingFee
                            || x.Type == FeeType.OtherStartingFees);
        }

        public static IEnumerable<FeeModel> GetAnnualFees(IEnumerable<FeeModel> allFees)
        {
            return allFees.Where(x => x.Type == FeeType.AnnualManagementFee
                            || x.Type == FeeType.OtherAnnualFees);
        }

        public static IEnumerable<FeeModel> GetMonthlyFees(IEnumerable<FeeModel> allFees)
        {
            return allFees.Where(x => x.Type == FeeType.MonthlyManagementFee
                            || x.Type == FeeType.OtherMonthlyFees);
        }

       
    }
}

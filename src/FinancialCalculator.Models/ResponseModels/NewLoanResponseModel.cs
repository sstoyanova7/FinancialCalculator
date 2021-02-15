namespace FinancialCalculator.Models.ResponseModels
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class NewLoanResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public decimal AnnualPercentCost { get; set; } //in percent
        public decimal TotalCost { get; set; }
        public decimal FeesCost { get; set; }
        public decimal InterestsCost { get; set; }
        public decimal InstallmentsCost { get; set; }
        public List<InstallmentForRepaymentPlanModel> RepaymentPlan { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is NewLoanResponseModel b))
                return false;
            return Status == b.Status
                && ErrorMessage == b.ErrorMessage
                && AnnualPercentCost == b.AnnualPercentCost
                && TotalCost == b.TotalCost
                && FeesCost == b.FeesCost
                && InterestsCost == b.InterestsCost
                && InstallmentsCost == b.InstallmentsCost;
                //&& RepaymentPlan.All(x => b.RepaymentPlan.Any(y => x .Equals(y)))
                //&& b.RepaymentPlan.All(x => RepaymentPlan.Any(y => x.Equals(y)));                
        }

        public override string ToString()
        {
            return String.Format("TotalCost: {0}, FeesCost: {1}, InterestsCost: {2}, InstallmentsCost: {3}, AnnualPercentCost: {4}",
                                TotalCost, FeesCost, InterestsCost, InstallmentsCost, AnnualPercentCost);
        }
    }
}

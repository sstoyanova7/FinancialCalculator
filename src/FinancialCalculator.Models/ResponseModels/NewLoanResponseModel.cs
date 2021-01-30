namespace FinancialCalculator.Models.ResponseModels
{
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
    }
}

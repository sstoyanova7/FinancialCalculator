using System.Collections.Generic;
using System.Net;

namespace FinancialCalculator.Models.ResponseModels
{
    public class NewLoanResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public decimal AnnualPercentCost { get; set; } //in percent
        public decimal TotalCost { get; set; }
        public decimal FeesCost { get; set; }
        public decimal InterestsCost { get; set; }
        public decimal InstallmentsCost { get; set; }
        List<InstallmentForRepaymentPlanModel> RepaymentPlan { get; set; }
    }
}

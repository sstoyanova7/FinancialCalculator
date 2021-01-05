using System.Net;

namespace FinancialCalculator.Models.ResponseModels
{
    public class LeasingLoanResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public decimal AnnualPercentCost { get; set; } //in percent
        public decimal TotalCost { get; set; } 
        public decimal TotalFees { get; set; }
    }
}

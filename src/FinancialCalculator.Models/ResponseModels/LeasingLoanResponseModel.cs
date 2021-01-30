namespace FinancialCalculator.Models.ResponseModels
{
    using System.Net;

    public class LeasingLoanResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public decimal AnnualPercentCost { get; set; } //in percent
        public decimal TotalCost { get; set; }
        public decimal TotalFees { get; set; }
    }
}

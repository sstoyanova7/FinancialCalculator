namespace FinancialCalculator.Models.RequestModels
{
    public class LoanBaseRequestModel
    {
        public string UserAgent { get; set; }
        public decimal LoanAmount { get; set; }
        public int Period { get; set; } //in months
        public decimal Interest { get; set; } //in percent
    }
}

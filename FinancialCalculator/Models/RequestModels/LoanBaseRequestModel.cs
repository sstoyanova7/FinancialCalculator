namespace FinancialCalculator.Models.RequestModels
{
    public class LoanBaseRequestModel
    {
        public decimal LoanAmount { get; set; }
        public int Period { get; set; } //in months
        public decimal Interest { get; set; } //in percent
    }
}

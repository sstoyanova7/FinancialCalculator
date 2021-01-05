namespace FinancialCalculator.Models.RequestModels
{
    public class LoanBaseRequestModel
    {
        public decimal LoanSize { get; set; }
        public int Period { get; set; } //in months
        public decimal Interest { get; set; } //in percent
    }
}

namespace FinancialCalculator.Models.ResponseModels
{
    public class RefinancingLoanHelperModel
    {
        public decimal Interest { get; set; } //in percent
        public int Period { get; set; } //in months
        public decimal EarlyInstallmentsFee { get; set; } // how the fuck is this calculated
        public decimal MonthlyInstallment { get; set; } // how the fuck is this calculated
        public decimal Total { get; set; } // how the fuck is this calculated
    }
}
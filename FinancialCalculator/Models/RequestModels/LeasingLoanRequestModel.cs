namespace FinancialCalculator.Models.RequestModels
{
    public class LeasingLoanRequestModel
    {
        public decimal Price { get; set; }
        public decimal StartingInstallment { get; set; } 
        public int Period { get; set; } //in months
        public decimal Installment { get; set; }
        public FeeModel StartingFee { get; set; }
    }
}

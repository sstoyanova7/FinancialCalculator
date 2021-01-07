using FinancialCalculator.Enums;

namespace FinancialCalculator.Models.RequestModels
{
    public class LeasingLoanRequestModel
    {
        public decimal ProductPrice { get; set; }
        public decimal StartingInstallment { get; set; } 
        public int Period { get; set; } //in months
        public decimal MonthlyInstallment { get; set; }
        public FeeModel StartingFee { get; set; }

        public override string ToString()
        {
            var feeValueTypeStr = StartingFee.ValueType == FeeValueType.Percent ? "%" : "lv";
            return @$"ProductPrice: {ProductPrice}, StartingInstallment: {StartingInstallment}, Period: {Period} months, 
                        MonthlyInstallment: {MonthlyInstallment}, StartingFee: {StartingFee.Value} {feeValueTypeStr}";
        }
    }
}

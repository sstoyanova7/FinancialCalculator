namespace FinancialCalculator.Models.RequestModels
{
    using Enums;

    public class LeasingLoanRequestModel
    {
        public string UserAgent { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal StartingInstallment { get; set; }
        public int Period { get; set; } //in months
        public decimal MonthlyInstallment { get; set; }
        public FeeModel StartingFee { get; set; }

        public override string ToString()
        {
            string feeValueTypeStr = null;
            if (StartingFee != null)
            {
                feeValueTypeStr = StartingFee.ValueType == FeeValueType.Percent ? "%" : "lv";
                return @$"ProductPrice: {ProductPrice}, StartingInstallment: {StartingInstallment}, Period: {Period} months, MonthlyInstallment: {MonthlyInstallment}, StartingFee: {StartingFee.Value} {feeValueTypeStr}";
            } else
            {
                return @$"ProductPrice: {ProductPrice}, StartingInstallment: {StartingInstallment}, Period: {Period} months, MonthlyInstallment: {MonthlyInstallment}";
            }

        }
    }
}

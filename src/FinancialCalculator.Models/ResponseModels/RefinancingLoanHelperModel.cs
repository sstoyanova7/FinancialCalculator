namespace FinancialCalculator.Models.ResponseModels
{
    public class RefinancingLoanHelperModel
    {
        public decimal Interest { get; set; } //in percent
        public int Period { get; set; } //in months
        public decimal EarlyInstallmentsFee { get; set; } = 0;
        public decimal MonthlyInstallment { get; set; }
        public decimal Total { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is RefinancingLoanHelperModel b))
                return false;
            return Interest == b.Interest
                && Period == b.Period
                && EarlyInstallmentsFee == b.EarlyInstallmentsFee
                && MonthlyInstallment == b.MonthlyInstallment
                && Total == b.Total;
        }
    }
}
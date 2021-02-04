namespace FinancialCalculator.Models.ResponseModels
{
    using System;

    public class InstallmentForRepaymentPlanModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public decimal PrincipalInstallment { get; set; }
        public decimal InterestInstallment { get; set; }
        public decimal PrincipalBalance { get; set; }
        public decimal Fees { get; set; }
        public decimal CashFlow { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is InstallmentForRepaymentPlanModel b))
                return false;
            return Id == b.Id
                && Date == b.Date
                && MonthlyInstallment == b.MonthlyInstallment
                && PrincipalInstallment == b.PrincipalInstallment
                && InterestInstallment == b.InterestInstallment
                && PrincipalBalance == b.PrincipalBalance
                && Fees == b.Fees
                && CashFlow == b.CashFlow;
        }
    }
}
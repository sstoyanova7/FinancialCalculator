using System;

namespace FinancialCalculator.Models.ResponseModels
{
    public class InstallmentForRepaymentPlanModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public decimal PrincipalInstallment { get; set; }
        public decimal InterestInstallment { get; set; }
        public decimal PrincipalBalance { get; set; }
        public decimal Fees { get; set; }
        public decimal CashFlow { get; set; } // ??

    }
}
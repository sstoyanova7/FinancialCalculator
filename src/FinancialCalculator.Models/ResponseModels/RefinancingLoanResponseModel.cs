namespace FinancialCalculator.Models.ResponseModels
{
    using System.Net;

    public class RefinancingLoanResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public RefinancingLoanHelperModel CurrentLoan { get; set; }
        public RefinancingLoanHelperModel NewLoan { get; set; }
        public decimal MonthlySavings => CurrentLoan.MonthlyInstallment - NewLoan.MonthlyInstallment;
        public decimal TotalSavings => CurrentLoan.Total - NewLoan.Total;

        public override bool Equals(object obj)
        {
            if (!(obj is RefinancingLoanResponseModel b))
                return false;
            return Status == b.Status
                && ErrorMessage == b.ErrorMessage
                && CurrentLoan == b.CurrentLoan
                && NewLoan == b.NewLoan
                && MonthlySavings == b.MonthlySavings
                && TotalSavings == b.TotalSavings;
        }
    }
}

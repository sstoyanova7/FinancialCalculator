using System.Net;

namespace FinancialCalculator.Models.ResponseModels
{
    public class RefinancingLoanResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public RefinancingLoanHelperModel CurrentLoan { get; set; }
        public RefinancingLoanHelperModel NewLoan { get; set; }
        public decimal MonthlySavings => CurrentLoan.MonthlyInstallment - NewLoan.MonthlyInstallment;
        public decimal TotalSavings => CurrentLoan.Total - NewLoan.Total;
    }
}

namespace FinancialCalculator.Models.ResponseModels
{
    using System.Net;

    public class LeasingLoanResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public decimal AnnualPercentCost { get; set; } //in percent
        public decimal TotalCost { get; set; }
        public decimal TotalFees { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is LeasingLoanResponseModel b))
                return false;
            return Status == b.Status
                && ErrorMessage == b.ErrorMessage
                && AnnualPercentCost == b.AnnualPercentCost
                && TotalCost == b.TotalCost
                && TotalFees == b.TotalFees;
        }
    }
}

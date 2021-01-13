namespace FinancialCalculator.Models.RequestModels
{
    using Enums;
    using System.Collections.Generic;
    using System.Linq;

    public class NewLoanRequestModel : LoanBaseRequestModel
    {
        public Installments InstallmentType { get; set; }
        public int PromoPeriod { get; set; } //in months
        public decimal PromoInterest { get; set; } //in percent
        public int GracePeriod { get; set; } //in months        
        public List<FeeModel> Fees { get; set; }

        public override string ToString()
        {
            var feesString = "No fees.";

            if (Fees.Any())
            {
                feesString = "Fees: ";
                foreach (var f in Fees)
                {
                    feesString += f;
                }
            }

            return $"InstallmentType: {InstallmentType}, PromoPeriod: {PromoPeriod} months, PromoInterest: {PromoInterest} %, GracePeriod: {GracePeriod} months, {feesString}";
        }
    }
}

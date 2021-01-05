using FinancialCalculator.Enums;
using System.Collections.Generic;

namespace FinancialCalculator.Models.RequestModels
{
    public class NewLoanRequestModel : LoanBaseRequestModel
    {
        public Installments InstallmentType { get; set; }
        public int PromoPeriod { get; set; } //in months
        public decimal PromoInterest { get; set; } //in percent
        public int GracePeriod { get; set; } //in months        
        public List<FeeModel> Fees { get; set; }
    }
}

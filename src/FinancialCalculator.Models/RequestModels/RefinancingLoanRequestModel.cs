﻿namespace FinancialCalculator.Models.RequestModels
{
    public class RefinancingLoanRequestModel : LoanBaseRequestModel
    {
        public int CountOfPaidInstallments { get; set; }
        public decimal EarlyInstallmentsFee { get; set; } //in percent
        public decimal NewInterest { get; set; } //in percent
        public decimal StartingFeesCurrency { get; set; }
        public decimal StartingFeesPercent { get; set; }

    }
}
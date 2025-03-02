﻿namespace FinancialCalculator.Models.ResponseModels
{
    using System;
    using System.Net;

    public class RefinancingLoanResponseModel
    {
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public RefinancingLoanHelperModel CurrentLoan { get; set; }
        public RefinancingLoanHelperModel NewLoan { get; set; }
        public decimal MonthlySavings => CurrentLoan?.MonthlyInstallment ?? 0 - NewLoan?.MonthlyInstallment ?? 0;
        public decimal TotalSavings => CurrentLoan?.Total ?? 0 - NewLoan?.Total ?? 0;

        public override bool Equals(object obj)
        {
            if (!(obj is RefinancingLoanResponseModel b))
                return false;
            return Status == b.Status
                && ErrorMessage == b.ErrorMessage
                && CurrentLoan.Equals(b.CurrentLoan)
                && NewLoan.Equals(b.NewLoan)
                && MonthlySavings == b.MonthlySavings
                && TotalSavings == b.TotalSavings;
        }
        public override string ToString()
        {
            return String.Format("MonthlySavings: {0}, TotalSavings: {1}, CurrentLoan: {2}, NewLoan: {3}",
                                MonthlySavings, TotalSavings, CurrentLoan, NewLoan);
        }
    }
}

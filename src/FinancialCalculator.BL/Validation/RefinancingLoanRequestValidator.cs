namespace FinancialCalculator.BL.Validation
{
    using FinancialCalculator.Models.RequestModels;

    public class RefinancingLoanRequestValidator : IValidator<RefinancingLoanRequestModel>
    {     
        public ValidationResult Validate(RefinancingLoanRequestModel request)
        {
            var validated = new ValidationResult();

            if (request == null)
            {
                validated.AddError("Request cannot be null.");
                return validated;
            }

            if (request.LoanAmount <= 0)
            {
                validated.AddError("Loan amount must be higher than zero.");
            }

            if(request.Period < 1)
            {
                validated.AddError("Period must be more than 1 month.");
            }

            if(request.Interest < 0 || request.Interest > 100)
            {
                validated.AddError("Interest must be between 0 and 100.");
            }

            if(request.CountOfPaidInstallments < 0)
            {
                validated.AddError("Count of paid installments cannot be less than zero.");
            }

            if (request.CountOfPaidInstallments > request.Period)
            {
                validated.AddError("Count of paid installments cannot be greater than the period.");
            }

            if(request.EarlyInstallmentsFee < 0 || request.EarlyInstallmentsFee > 100)
            {
                validated.AddError("Early installments fee must be between 0 and 100.");
            }

            if(request.NewInterest < 0 || request.NewInterest > 100)
            {
                validated.AddError("New interest must be between 0 and 100.");
            }

            if(request.StartingFeesCurrency < 0)
            {
                validated.AddError("Starting fees currency cannot be less than zero.");
            } 
            
            if(request.StartingFeesPercent < 0 || request.StartingFeesPercent > 100)
            {
                validated.AddError("Starting fees percent must be between 0 and 100.");
            }

            return validated;
        }
    }
}

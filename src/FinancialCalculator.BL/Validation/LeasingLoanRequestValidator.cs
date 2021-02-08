namespace FinancialCalculator.BL.Validation
{
    using FinancialCalculator.Models.RequestModels;

    public class LeasingLoanRequestValidator : IValidator<LeasingLoanRequestModel>
    {
        private IValidator<FeeModel> _feeModelValidator;
        public LeasingLoanRequestValidator(IValidator<FeeModel> feeModelValidator)
        {
            _feeModelValidator = feeModelValidator;
        }

        public ValidationResult Validate(LeasingLoanRequestModel request)
        {
            var validated = new ValidationResult();

            if (request == null)
            {
                validated.AddError("Request cannot be null.");
                return validated;
            }

            if (request.ProductPrice <= 0)
            {
                validated.AddError("Product price must be higher than zero.");
            }

            if (request.StartingInstallment < 0)
            {
                validated.AddError("Starting installment cannot be less than zero.");
            }

            if(request.ProductPrice < request.StartingInstallment)
            {
                validated.AddError("Product price cannot be less than starting installment");
            }

            if (request.Period < 1)
            {
                validated.AddError("Period must be at least 1 month.");
            }

            if (request.MonthlyInstallment <= 0)
            {
                validated.AddError("Monthly installment must be greater than zero.");
            }

            var startingFeeValidated = _feeModelValidator.Validate(request.StartingFee);
            if (!startingFeeValidated.IsValid)
            {
                validated.AddError($"Starting fee is not valid. {startingFeeValidated}.");
                return validated;
            }

            return validated;
        }
    }
}

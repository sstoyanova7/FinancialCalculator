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

        public Validation<LeasingLoanRequestModel> Validate(LeasingLoanRequestModel request)
        {
            var validated = new Validation<LeasingLoanRequestModel>(request);

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

            if (request.Period < 1)
            {
                validated.AddError("Period must be more than 1 month.");
            }

            if (request.MonthlyInstallment <= 0)
            {
                validated.AddError("Monthly installment must be greater than zero.");
            }

            var startingFeeValidated = _feeModelValidator.Validate(request.StartingFee);
            if (!startingFeeValidated.IsValid)
            {
                validated.AddError($"Starting fee is not valid. {startingFeeValidated.GetValidationSummary()}.");
                return validated;
            }

            return validated;
        }
    }
}

namespace FinancialCalculator.BL.Validation
{
    using FinancialCalculator.Models.Enums;
    using FinancialCalculator.Models.RequestModels;
    using System;
    using System.Linq;

    public class NewLoanRequestValidator : IValidator<NewLoanRequestModel>
    {
        private IValidator<FeeModel> _feeModelValidator;
        public NewLoanRequestValidator(IValidator<FeeModel> feeModelValidator)
        {
            _feeModelValidator = feeModelValidator;
        }

        public Validation<NewLoanRequestModel> Validate(NewLoanRequestModel request)
        {
            var validated = new Validation<NewLoanRequestModel>(request);

            if (request == null)
            {
                validated.AddError("Request cannot be null.");
                return validated;
            }

            if (request.LoanAmount <= 0)
            {
                validated.AddError("Loan amount must be higher than zero.");
            }

            if (request.Period < 1)
            {
                validated.AddError("Period must be more than 1 month.");
            }

            if (request.Interest < 0 || request.Interest > 100)
            {
                validated.AddError("Interest must be between 0 and 100.");
            }

            var installments = Enum.GetValues(typeof(Installments));
            if (request.InstallmentType < installments.Cast<Installments>().First() || request.InstallmentType > installments.Cast<Installments>().Last())
            {
                var errorString = $"Installment type should be one of the following:";
                foreach (var i in installments)
                {
                    errorString += $"i({Enum.GetName(typeof(Installments), i)})";
                }
                validated.AddError(errorString);
            }

            if (request.PromoPeriod < 0)
            {
                validated.AddError("Promo period cannot be less than zero months.");
            }

            if (request.PromoPeriod >= request.Period)
            {
                validated.AddError("Promo period must be less than the whole period.");
            }

            if (request.PromoInterest < 0)
            {
                validated.AddError("Promo interest cannot be less than 0.");
            }

            if (request.PromoInterest > request.Interest)
            {
                validated.AddError("Promo interest cannot be lhigher than original interest");
            }

            if (request.GracePeriod < 0)
            {
                validated.AddError("Grace period cannot be less than zero months.");
            }

            if (request.GracePeriod >= request.Period)
            {
                validated.AddError("Grace period must be less than the whole period.");
            }

            var invalidFees = request.Fees.Select(x => _feeModelValidator.Validate(x)).Where(x => !x.IsValid);

            foreach (var fee in invalidFees)
            {
                validated.AddError(fee.GetValidationSummary());
            }

            return validated;
        }
    }
}

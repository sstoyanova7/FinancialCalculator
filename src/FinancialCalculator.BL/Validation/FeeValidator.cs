namespace FinancialCalculator.BL.Validation
{
    using FinancialCalculator.Models.Enums;
    using FinancialCalculator.Models.RequestModels;
    using System;
    using System.Linq;

    public class FeeValidator : IValidator<FeeModel>
    {
        public ValidationResult Validate(FeeModel fee)
        {
            var validated = new ValidationResult();

            if (fee == null)
            {
                return validated;
            }

            var feeTypes = Enum.GetValues(typeof(FeeType));
            if (fee.Type < feeTypes.Cast<FeeType>().First() || fee.Type > feeTypes.Cast<FeeType>().Last())
            {
                var errorString = $"Fee type should be one of the following:";
                foreach (var i in feeTypes)
                {
                    errorString += $"i({Enum.GetName(typeof(FeeType), i)}) ";
                }
                validated.AddError(errorString);
            }

            if (fee.Value < 0)
            {
                validated.AddError($"{fee.Type} value cannot be less than zero.");
            }

            var feeValueTypes = Enum.GetValues(typeof(FeeValueType));
            if (fee.ValueType < feeValueTypes.Cast<FeeValueType>().First() || fee.ValueType > feeValueTypes.Cast<FeeValueType>().Last())
            {
                var errorString = $"{fee.Type} value type should be one of the following:";
                foreach (var i in feeValueTypes)
                {
                    errorString += $"{i}({Enum.GetName(typeof(FeeValueType), i)}) ";
                }
                validated.AddError(errorString);
            }

            return validated;
        }

    }
}
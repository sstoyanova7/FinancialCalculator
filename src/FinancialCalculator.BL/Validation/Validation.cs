namespace FinancialCalculator.BL.Validation
{
    using System.Collections.Generic;

    public class Validation<T>
    {
        public Validation(T validated)
        {
            Validated = validated;
        }
        public T Validated { get; }

        public bool IsValid { get; private set; } = true;

        private List<string> Errors { get; } = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
            IsValid = false;
        }

        public string GetValidationSummary()
        {
            return string.Join(".", Errors);
        }

        public Validation<T> Validate(T objToValidate)
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace FinancialCalculator.BL.Validation
{
    using System.Collections.Generic;

    public class ValidationResult
    {
        public bool IsValid { get; private set; } = true;

        private List<string> Errors { get; } = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
            IsValid = false;
        }

        public override string ToString()
        {
            return string.Join(".", Errors);
        }

    }
}

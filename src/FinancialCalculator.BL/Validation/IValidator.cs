namespace FinancialCalculator.BL.Validation
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T  objToValidate);
    }
}
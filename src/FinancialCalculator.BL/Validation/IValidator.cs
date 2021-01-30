namespace FinancialCalculator.BL.Validation
{
    public interface IValidator<T>
    {
        Validation<T> Validate(T  objToValidate);
    }
}
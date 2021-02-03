namespace FinancialCalculator.Services
{
    using Models.RequestModels;
    using Models.ResponseModels;

    public interface ICalculatorService<T1, T2>
    {
        T1 Calculate(T2 requestModel);       
    }
}

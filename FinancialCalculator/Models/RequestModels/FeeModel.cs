using FinancialCalculator.Enums;

namespace FinancialCalculator.Models.RequestModels
{
    public class FeeModel
    {
        public FeeType Type { get; set; }
        public decimal Value { get; set; }
        public FeeValueType ValueType { get; set; }
    }
}

namespace FinancialCalculator.Models.RequestModels
{
    using Enums;

    public class FeeModel
    {
        public FeeType Type { get; set; }
        public decimal Value { get; set; }
        public FeeValueType ValueType { get; set; }

        public override string ToString()
        {
            var feeValueTypeStr = ValueType == FeeValueType.Percent ? "%" : "lv";
            return $"FeeType: {Type}, Value: {Value} {feeValueTypeStr}; ";
        }
    }
}

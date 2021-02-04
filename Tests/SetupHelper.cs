namespace Tests
{
    using FinancialCalculator.BL.Validation;
    using Moq;

    public static class SetupHelper
    {
        public static Mock<IValidator<T>> CreateValidatorGeneric<T>()
        {
            var mock = new Mock<IValidator<T>>();

            mock.Setup(l => l.Validate(It.IsAny<T>())).Returns(new ValidationResult());

            return mock;
        }
    }
}

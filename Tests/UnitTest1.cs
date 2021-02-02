namespace Tests
{
    using FinancialCalculator.BL.Validation;
    using FinancialCalculator.Models.Enums;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using FinancialCalculator.Services;
    using Moq;
    using NUnit.Framework;
    using NUnit.Framework.Internal;

    public class Tests
    {
        private ICalculatorService _service;

        private Mock<Serilog.ILogger> _logger;
        private Mock<IValidator<LeasingLoanRequestModel>> _leasingLoanValidator;
        private Mock<IValidator<NewLoanRequestModel>> _newLoanValidator;
        private Mock<IValidator<RefinancingLoanRequestModel>> _refinancingLoanValidator;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<Serilog.ILogger>();
            _leasingLoanValidator = CreateValidatorGeneric<LeasingLoanRequestModel>();
            _newLoanValidator = CreateValidatorGeneric<NewLoanRequestModel>();
            _refinancingLoanValidator =  CreateValidatorGeneric<RefinancingLoanRequestModel>();
            _service = new CalculatorService(_logger.Object, _leasingLoanValidator.Object, _newLoanValidator.Object, _refinancingLoanValidator.Object);
        }

        public Mock<IValidator<T>> CreateValidatorGeneric<T>()
        {
            var mock = new Mock<IValidator<T>>();

            mock.Setup(l => l.Validate(It.IsAny<T>())).Returns(new ValidationResult());

            return mock;
        } 
            


        [Test]
        public void Test1()
        {
            var requestModel = new LeasingLoanRequestModel
            {
                ProductPrice = 1000,
                StartingInstallment = 100,
                Period = 12,
                MonthlyInstallment = 100,
                StartingFee = new FeeModel
                {
                    Type = FeeType.StartingProcessingFee,
                    ValueType = FeeValueType.Currency,
                    Value = 2
                }
            };
            var actual = _service.CalculateLeasingLoan(requestModel);

            var expected = new LeasingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.OK,
                AnnualPercentCost = 0,
                TotalCost = 1302,
                TotalFees = 2
            };

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
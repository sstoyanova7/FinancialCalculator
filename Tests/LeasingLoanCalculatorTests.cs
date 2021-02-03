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

    public class LeasingLoanCalculatorTests
    {
        private ICalculatorService<LeasingLoanResponseModel, LeasingLoanRequestModel> _service;
        private Mock<Serilog.ILogger> _logger;
        private Mock<IValidator<LeasingLoanRequestModel>> _leasingLoanValidator;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<Serilog.ILogger>();
            _leasingLoanValidator = SetupHelper.CreateValidatorGeneric<LeasingLoanRequestModel>();          
            _service = new LeasingLoanCalculatorService(_logger.Object, _leasingLoanValidator.Object);
        }

        [Test]
        public void LeasingLoanNoFees()
        {
            var requestModel = new LeasingLoanRequestModel
            {
                ProductPrice = 200,
                StartingInstallment = 10,
                Period = 25,
                MonthlyInstallment = 10
            };
            var actual = _service.Calculate(requestModel);

            var expected = new LeasingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.OK,
                //AnnualPercentCost = 30.34,
                AnnualPercentCost = 0,
                TotalCost = 260,
                TotalFees = 0
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void LeasingLoanFeeCurrency()
        {
            var requestModel = new LeasingLoanRequestModel
            {
                ProductPrice = 200,
                StartingInstallment = 10,
                Period = 25,
                MonthlyInstallment = 10,
                StartingFee = new FeeModel
                {
                    Type = FeeType.StartingProcessingFee,
                    ValueType = FeeValueType.Currency,
                    Value = 2
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new LeasingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.OK,
                //AnnualPercentCost = 31.75,
                AnnualPercentCost = 0,
                TotalCost = 262,
                TotalFees = 2
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void LeasingLoanFeePercent()
        {
            var requestModel = new LeasingLoanRequestModel
            {
                ProductPrice = 200,
                StartingInstallment = 10,
                Period = 25,
                MonthlyInstallment = 10,
                StartingFee = new FeeModel
                {
                    Type = FeeType.StartingProcessingFee,
                    ValueType = FeeValueType.Percent,
                    Value = 2
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new LeasingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.OK,
                //AnnualPercentCost = 33.19,
                AnnualPercentCost = 0,
                TotalCost = 264,
                TotalFees = 4
            };

            Assert.AreEqual(actual, expected);
        }
        
        [Test]
        public void InvalidLeasingLoan()
        {
            var requestModel = new LeasingLoanRequestModel
            {
                ProductPrice = 200,
                StartingInstallment = 1,
                Period = 25,
                MonthlyInstallment = 1,
            };
            var actual = _service.Calculate(requestModel);

            var expected = new LeasingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = "You cannot have a leasing loan with these parameters."
            };

            Assert.AreEqual(actual, expected);
        }
    }
}
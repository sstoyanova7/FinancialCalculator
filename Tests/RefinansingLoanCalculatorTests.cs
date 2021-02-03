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

    public class RefinansingLoanCalculatorTests
    {
        private ICalculatorService<RefinancingLoanResponseModel, RefinancingLoanRequestModel> _service;
        private Mock<Serilog.ILogger> _logger;
        private Mock<IValidator<RefinancingLoanRequestModel>> _refinancingLoanValidator;
        private Mock<IValidator<NewLoanRequestModel>> _newLoanValidator;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<Serilog.ILogger>();
            _refinancingLoanValidator = SetupHelper.CreateValidatorGeneric<RefinancingLoanRequestModel>();
            _newLoanValidator = SetupHelper.CreateValidatorGeneric<NewLoanRequestModel>();
            var _newLoanService = new NewLoanCalculatorService(_logger.Object, _newLoanValidator.Object);
            
            _service = new RefinancingLoanCalculatorService(_logger.Object, _refinancingLoanValidator.Object, _newLoanService);
        }       

        [Test]
        public void RefinancingLoanNoFees()
        {
            var requestModel = new RefinancingLoanRequestModel
            {
                LoanAmount = 1000,
                Interest = 10,
                Period = 12,
                CountOfPaidInstallments = 2,
                EarlyInstallmentsFee = 0,
                NewInterest = 2,
                StartingFeesCurrency = 0,
                StartingFeesPercent = 0
            };
            var actual = _service.Calculate(requestModel);

            var expected = new RefinancingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.OK,
                CurrentLoan = new RefinancingLoanHelperModel
                {
                    Interest = 10,
                    Period = 12,
                    EarlyInstallmentsFee = 0,
                    MonthlyInstallment = 87.92M,
                    //Total = 879.16M
                    Total = 879.20M

                },
                NewLoan = new RefinancingLoanHelperModel
                {
                    Interest = 2,
                    Period = 10,
                    EarlyInstallmentsFee = 0,
                    //MonthlyInstallment = 84.79M,
                    MonthlyInstallment = 84.77M,
                    //Total = 847.89M
                    Total = 847.70M
                }
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void RefinancingLoanWithEarlyInstallmentsFee()
        {
            var requestModel = new RefinancingLoanRequestModel
            {
                LoanAmount = 1000,
                Interest = 10,
                Period = 12,
                CountOfPaidInstallments = 2,
                EarlyInstallmentsFee = 2,
                NewInterest = 2,
                StartingFeesCurrency = 0,
                StartingFeesPercent = 0
            };
            var actual = _service.Calculate(requestModel);

            var expected = new RefinancingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.OK,
                CurrentLoan = new RefinancingLoanHelperModel
                {
                    Interest = 10,
                    Period = 12,
                    EarlyInstallmentsFee = 16.80M,
                    MonthlyInstallment = 87.92M,
                    //Total = 879.16M
                    Total = 879.20M

                },
                NewLoan = new RefinancingLoanHelperModel
                {
                    Interest = 2,
                    Period = 10,
                    EarlyInstallmentsFee = 0,
                    //MonthlyInstallment = 84.79M,
                    MonthlyInstallment = 84.77M,
                    //Total = 864.70M
                    Total = 864.50M
                }
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void RefinancingLoanWithStartingFees()
        {
            var requestModel = new RefinancingLoanRequestModel
            {
                LoanAmount = 1000,
                Interest = 10,
                Period = 12,
                CountOfPaidInstallments = 2,
                EarlyInstallmentsFee = 0,
                NewInterest = 2,
                StartingFeesCurrency = 2,
                StartingFeesPercent = 2
            };
            var actual = _service.Calculate(requestModel);

            var expected = new RefinancingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.OK,
                CurrentLoan = new RefinancingLoanHelperModel
                {
                    Interest = 10,
                    Period = 12,
                    EarlyInstallmentsFee = 0,
                    MonthlyInstallment = 87.92M,
                    //Total = 879.16M
                    Total = 879.20M
                },
                NewLoan = new RefinancingLoanHelperModel
                {
                    Interest = 2,
                    Period = 10,
                    EarlyInstallmentsFee = 0,
                    //MonthlyInstallment = 84.79M,
                    MonthlyInstallment = 84.77M,
                    //Total = 869.89M
                    Total = 868.50M
                }
            };

            Assert.AreEqual(actual, expected);
        }

    }
}
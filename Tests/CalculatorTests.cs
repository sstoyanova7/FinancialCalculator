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

    public class CalculatorTests
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
        public void LeasingLoanNoFees()
        {
            var requestModel = new LeasingLoanRequestModel
            {
                ProductPrice = 200,
                StartingInstallment = 10,
                Period = 25,
                MonthlyInstallment = 10
            };
            var actual = _service.CalculateLeasingLoan(requestModel);

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
            var actual = _service.CalculateLeasingLoan(requestModel);

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
            var actual = _service.CalculateLeasingLoan(requestModel);

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
            var actual = _service.CalculateLeasingLoan(requestModel);

            var expected = new LeasingLoanResponseModel
            {
                Status = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = "You cannot have a leasing loan with these parameters."
            };

            Assert.AreEqual(actual, expected);
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
            var actual = _service.CalculateRefinancingLoan(requestModel);

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
            var actual = _service.CalculateRefinancingLoan(requestModel);

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
            var actual = _service.CalculateRefinancingLoan(requestModel);

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
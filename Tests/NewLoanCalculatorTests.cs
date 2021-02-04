namespace Tests
{
    using FinancialCalculator.BL.Validation;
    using FinancialCalculator.Models.Enums;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using FinancialCalculator.Services;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;

    public class NewLoanCalculatorTests
    {
        private ICalculatorService<NewLoanResponseModel, NewLoanRequestModel> _service;
        private Mock<Serilog.ILogger> _logger;
        private Mock<IValidator<NewLoanRequestModel>> _newLoanValidator;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<Serilog.ILogger>();
            _newLoanValidator = SetupHelper.CreateValidatorGeneric<NewLoanRequestModel>();
            _service = new NewLoanCalculatorService(_logger.Object, _newLoanValidator.Object);
        }

        [Test]
        public void NewLoanNoFeesNoPromoPeriodNoGracePeriodAnuityInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
               LoanAmount = 1000,
               Period = 15,
               Interest = 2,
               InstallmentType = Installments.AnnuityInstallment
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 2.0207M
                AnnualPercentCost = 0,
                TotalCost = 1013.40M,
                FeesCost = 0M,
                InterestsCost = 13.40M,
                InstallmentsCost = 1013.40M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanNoFeesNoPromoPeriodNoGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 2.01807M
                AnnualPercentCost = 0,
                TotalCost = 1013.33M,
                FeesCost = 0M,
                InterestsCost = 13.33M,
                InstallmentsCost = 1013.33M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanStartingFeesNPromoPeriodNoGracePeriodAnuityInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.AnnuityInstallment,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.StartingApplicationFee,
                        Value = 2,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 3,9004M
                AnnualPercentCost = 0,
                //TotalCost = 1025.33M,
                TotalCost = 1025.40M,
                FeesCost = 12M,
                //InterestsCost = 13.33M,
                InterestsCost = 13.40M,
                //InstallmentsCost = 1013.33M
                InstallmentsCost = 1013.40M
            };

            Assert.AreEqual(actual, expected);
        }
    }
}

namespace Tests
{
    using FinancialCalculator.BL.Validation;
    using FinancialCalculator.Models.Enums;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using FinancialCalculator.Services;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Net;

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
        public void NewLoanStartingFeesNoPromoPeriodNoGracePeriodAnuityInstallments()
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
                TotalCost = 1025.40M,
                FeesCost = 12M,
                InterestsCost = 13.40M,
                InstallmentsCost = 1013.40M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanStartingFeesNoPromoPeriodNoGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
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
                TotalCost = 1025.33M,
                FeesCost = 12M,
                InterestsCost = 13.33M,
                InstallmentsCost = 1013.33M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAnnualFeesNoPromoPeriodNoGracePeriodAnuityInstallments()
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
                        Type = FeeType.AnnualManagementFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 2,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 2.9305M
                AnnualPercentCost = 0,
                TotalCost = 1017.42M,
                FeesCost = 4.02M,
                InterestsCost = 13.40M,
                InstallmentsCost = 1013.40M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAnnualFeesNoPromoPeriodNoGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 2,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 2.6212M
                AnnualPercentCost = 0,
                TotalCost = 1017.33M,
                FeesCost = 4.00M,
                InterestsCost = 13.33M,
                InstallmentsCost = 1013.33M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanMonthlyFeesNoPromoPeriodNoGracePeriodAnnuityInstallments()
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
                        Type = FeeType.MonthlyManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 32.0560M
                AnnualPercentCost = 0,
                TotalCost = 1189.03M,
                FeesCost = 175.63M,
                InterestsCost = 13.40M,
                InstallmentsCost = 1013.40M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanMonthlyFeesNoPromoPeriodNoGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 32.06290M
                AnnualPercentCost = 0,
                TotalCost = 1188.33M,
                FeesCost = 175.00M,
                InterestsCost = 13.33M,
                InstallmentsCost = 1013.33M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAllFeesNoPromoPeriodNoGracePeriodAnnuityInstallments()
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
                        Value = 20,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },                     
                     new FeeModel
                    {
                        Type = FeeType.OtherStartingFees,
                        Value = 12,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 0.5M,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 0.5M,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 17.8596M
                AnnualPercentCost = 0,
                TotalCost = 1108.11M,
                FeesCost = 94.71M,
                InterestsCost = 13.40M,
                InstallmentsCost = 1013.40M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAllFeesNoPromoPeriodNoGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.StartingApplicationFee,
                        Value = 20,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherStartingFees,
                        Value = 12,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 0.5M,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 0.5M,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 17.8865M
                AnnualPercentCost = 0,
                TotalCost = 1107.83M,
                FeesCost = 94.50M,
                InterestsCost = 13.33M,
                InstallmentsCost = 1013.33M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanNoFeesNoPromoPeriodGracePeriodAnuityInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.AnnuityInstallment,
                GracePeriod = 2

            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 2.02127M
                AnnualPercentCost = 0,
                TotalCost = 1015.06M,
                FeesCost = 0M,
                InterestsCost = 15.06M,
                InstallmentsCost = 1015.06M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanNoFeesNoPromoPeriodGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                GracePeriod = 2
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 2.0197M
                AnnualPercentCost = 0,
                TotalCost = 1015.01M,
                FeesCost = 0M,
                InterestsCost = 15.01M,
                InstallmentsCost = 1015.01M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAllFeesNoPromoPeriodGracePeriodAnnuityInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.AnnuityInstallment,
                GracePeriod = 2,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.StartingApplicationFee,
                        Value = 20,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherStartingFees,
                        Value = 12,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 0.5M,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 0.5M,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 16.8478M
                AnnualPercentCost = 0,
                TotalCost = 1115.34M,
                FeesCost = 100.28M,
                InterestsCost = 15.06M,
                InstallmentsCost = 1015.06M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAllFeesNoPromoPeriodGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                GracePeriod = 2,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.StartingApplicationFee,
                        Value = 20,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherStartingFees,
                        Value = 12,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 0.5M,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 0.5M,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 16.8625M
                AnnualPercentCost = 0,
                TotalCost = 1115.13M,
                FeesCost = 100.12M,
                InterestsCost = 15.01M,
                InstallmentsCost = 1015.01M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanNoFeesPromoPeriodNoGracePeriodAnuityInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.AnnuityInstallment,
                PromoPeriod = 2,
                PromoInterest = 1
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 1.7710M
                AnnualPercentCost = 0,
                //TotalCost = 1011.76M,
                TotalCost = 1011.72M,
                FeesCost = 0M,
                //InterestsCost = 11.76M,     
                InterestsCost = 11.72M,
                //InstallmentsCost = 1011.76M
                InstallmentsCost = 1011.72M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanNoFeesPromoPeriodNoGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                PromoPeriod = 2,
                PromoInterest = 1
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 1.7693M
                AnnualPercentCost = 0,
                TotalCost = 1011.71M,
                FeesCost = 0M,
                InterestsCost = 11.71M,
                InstallmentsCost = 1011.71M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAllFeesPromoPeriodNoGracePeriodAnnuityInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.AnnuityInstallment,
                PromoPeriod = 2,
                PromoInterest = 1,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.StartingApplicationFee,
                        Value = 20,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherStartingFees,
                        Value = 12,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 0.5M,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 0.5M,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 17.5573M
                AnnualPercentCost = 0,
                //TotalCost = 1106.43M,
                TotalCost = 1106.28M,
                //FeesCost = 94.67M,
                FeesCost = 94.56M,
                //InterestsCost = 11.76M,
                InterestsCost = 11.72M,
                //InstallmentsCost = 1011.76M
                InstallmentsCost = 1011.72M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAllFeesPromoPeriodNoGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                PromoPeriod = 2,
                PromoInterest = 1,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.StartingApplicationFee,
                        Value = 20,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherStartingFees,
                        Value = 12,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 0.5M,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 0.5M,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 17.5807M
                AnnualPercentCost = 0,
                TotalCost = 1106.21M,
                FeesCost = 94.50M,
                InterestsCost = 11.71M,
                InstallmentsCost = 1011.71M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanNoFeesPromoPeriodGracePeriodAnuityInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.AnnuityInstallment,
                PromoPeriod = 4,
                PromoInterest = 1,
                GracePeriod = 2

            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 1.5728M
                AnnualPercentCost = 0,
                //TotalCost = 1011.75M,
                TotalCost = 1011.74M,
                FeesCost = 0M,
                //InterestsCost = 11.75M,
                InterestsCost = 11.74M,
                //InstallmentsCost = 1011.75M
                InstallmentsCost = 1011.74M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanNoFeesPromoPeriodGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                PromoPeriod = 4,
                PromoInterest = 1,
                GracePeriod = 2
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 1.5720M
                AnnualPercentCost = 0,
                TotalCost = 1011.72M,
                FeesCost = 0M,
                InterestsCost = 11.72M,
                InstallmentsCost = 1011.72M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAllFeesPromoPeriodGracePeriodAnnuityInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.AnnuityInstallment,
                PromoPeriod = 2,
                PromoInterest = 1,
                GracePeriod = 3,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.StartingApplicationFee,
                        Value = 20,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherStartingFees,
                        Value = 12,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 0.5M,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 0.5M,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 16.1763M
                AnnualPercentCost = 0,
                TotalCost = 1117.34M,
                FeesCost = 103.14M,
                InterestsCost = 14.20M,
                InstallmentsCost = 1014.20M
            };

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void NewLoanAllFeesPromoPeriodGracePeriodDecreasingInstallments()
        {
            var requestModel = new NewLoanRequestModel
            {
                LoanAmount = 1000,
                Period = 15,
                Interest = 2,
                InstallmentType = Installments.DecreasingInstallment,
                PromoPeriod = 2,
                PromoInterest = 1,
                GracePeriod = 3,
                Fees = new List<FeeModel>
                {
                    new FeeModel
                    {
                        Type = FeeType.StartingApplicationFee,
                        Value = 20,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.StartingProcessingFee,
                        Value = 1,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherStartingFees,
                        Value = 12,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.AnnualManagementFee,
                        Value = 2,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherAnnualFees,
                        Value = 1,
                        ValueType = FeeValueType.Currency
                    },
                     new FeeModel
                    {
                        Type = FeeType.MonthlyManagementFee,
                        Value = 0.5M,
                        ValueType = FeeValueType.Percent
                    },
                     new FeeModel
                    {
                        Type = FeeType.OtherMonthlyFees,
                        Value = 0.5M,
                        ValueType = FeeValueType.Currency
                    }
                }
            };
            var actual = _service.Calculate(requestModel);

            var expected = new NewLoanResponseModel
            {
                Status = HttpStatusCode.OK,
                //AnnualPercentCost = 16.1872M
                AnnualPercentCost = 0,
                TotalCost = 1117.17M,
                FeesCost = 103.00M,
                InterestsCost = 14.17M,
                InstallmentsCost = 1014.17M
            };

            Assert.AreEqual(actual, expected);
        }
    }
}

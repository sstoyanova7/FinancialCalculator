namespace FinancialCalculator.Services
{
    using FinancialCalculator.BL.Utilities;
    using FinancialCalculator.Models.Enums;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Serilog;
    using System.Net;

    public class CalculatorService : ICalculatorService
    {

        private readonly ILogger _logger;

        
        public CalculatorService(ILogger logger)
        {
            _logger = logger.ForContext<CalculatorService>();
        }

        public NewLoanResponseModel CalculateNewLoan(NewLoanRequestModel requestModel)
        {

            //TO DO: PromoPeriod % Promorate
            //TO DO: GracePeriod
            //TO DO: think of validations
            //TO DO: add exception handling
            //TO DO: check logs format
            //TO DO: calculate AnnualPercentCost ?!
            //TO DO: why the fuck the last monthly installment is different
            try
            {

                var plan = requestModel.InstallmentType == Installments.AnnuityInstallment
                        ? GetAnuityPlan(requestModel).ToList()
                        : GetDecreasingPlan(requestModel).ToList();

                var totalFeesCost = plan.Sum(x => x.Fees);
                var totalMonthlyInstallmentsCost = plan.Sum(x => x.MonthlyInstallment);
                var totalInterestCost = plan.Sum(x => x.InterestInstallment);
                return new NewLoanResponseModel
                {
                    Status = HttpStatusCode.OK,
                    AnnualPercentCost = CalcHelpers.CalculateAPR(totalFeesCost, totalInterestCost, requestModel.LoanAmount, requestModel.Period),
                    TotalCost = totalMonthlyInstallmentsCost + totalFeesCost,
                    FeesCost = totalFeesCost,
                    InterestsCost = totalInterestCost,
                    InstallmentsCost = totalMonthlyInstallmentsCost,
                    RepaymentPlan = plan
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occured while trying to calculate new loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new NewLoanResponseModel
                {
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        public RefinancingLoanResponseModel CalculateRefinancingLoan(RefinancingLoanRequestModel requestModel)
        {
            try
            {
                var currentLoanCalculated = CalculateNewLoan(
                    new NewLoanRequestModel
                    {
                        LoanAmount = requestModel.LoanAmount,
                        Period = requestModel.Period,
                        Interest = requestModel.Interest,
                        InstallmentType = Installments.AnnuityInstallment
                    });

                var periodLeft = requestModel.Period - requestModel.CountOfPaidInstallments;
                var monthlyInstallmentCurrentLoan = currentLoanCalculated.RepaymentPlan.First(x => x.Id == 1).MonthlyInstallment;
                var moneyLeftToBePaid = periodLeft * monthlyInstallmentCurrentLoan;
                var earlyInstallmentsFeeInCurrency = CalcHelpers.GetFeeCost(requestModel.EarlyInstallmentsFee, moneyLeftToBePaid);

                var newLoanCalculated = CalculateNewLoan(
                    new NewLoanRequestModel
                    {
                        LoanAmount = Math.Floor(moneyLeftToBePaid),
                        Period = periodLeft,
                        Interest = requestModel.NewInterest,
                        InstallmentType = Installments.AnnuityInstallment,
                        Fees = new List<FeeModel>
                        {
                            new FeeModel
                            {
                                Type = FeeType.StartingApplicationFee,
                                Value = requestModel.StartingFeesCurrency,
                                ValueType = FeeValueType.Currency
                            },
                            new FeeModel
                            {
                                Type = FeeType.OtherStartingFees,
                                Value = requestModel.StartingFeesPercent,
                                ValueType = FeeValueType.Percent
                            }
                        }                        
                    });

                var monthlyInstallmentNewLoan = newLoanCalculated.RepaymentPlan.First(x => x.Id == 1).MonthlyInstallment;

                return new RefinancingLoanResponseModel
                {
                    Status = HttpStatusCode.OK,
                    CurrentLoan = new RefinancingLoanHelperModel
                    {
                        Interest = requestModel.Interest,
                        Period = requestModel.Period,
                        EarlyInstallmentsFee = earlyInstallmentsFeeInCurrency,
                        MonthlyInstallment = monthlyInstallmentCurrentLoan,
                        Total = moneyLeftToBePaid
                    },
                    NewLoan = new RefinancingLoanHelperModel
                    {
                        Interest = requestModel.NewInterest,
                        Period = periodLeft,
                        MonthlyInstallment = monthlyInstallmentNewLoan,
                        Total = periodLeft * monthlyInstallmentNewLoan + earlyInstallmentsFeeInCurrency
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occured while trying to calculate new loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new RefinancingLoanResponseModel
                {
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        public LeasingLoanResponseModel CalculateLeasingLoan(LeasingLoanRequestModel requestModel)
        {
            //TO DO: AnnualPercentCost how to calculate ?
            try
            {
                var totalFees = CalcHelpers.GetFeeCost(requestModel.StartingFee, requestModel.ProductPrice);
                var totalCost = totalFees + requestModel.StartingInstallment + requestModel.Period * requestModel.MonthlyInstallment;


                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.OK,
                    //AnnualPercentCost
                    TotalCost = totalCost,
                    TotalFees = totalFees
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occured while trying to calculate leasing loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        private IEnumerable<InstallmentForRepaymentPlanModel>  GetDecreasingPlan(NewLoanRequestModel requestModel)
        {
            var principalInstallment = Math.Round(requestModel.LoanAmount / requestModel.Period, 2);
            var lastPrincipalInstallment = requestModel.LoanAmount - (requestModel.Period - 1) * principalInstallment;

            var installments = new Dictionary<int, InstallmentForRepaymentPlanModel>();
            var startingFees = CalcHelpers.CalculateStartingFeesCost(requestModel.LoanAmount, requestModel.Fees);

            var zero = new InstallmentForRepaymentPlanModel
            {
                Id = 0,
                Date = DateTime.UtcNow.Date,
                MonthlyInstallment = 0,
                PrincipalInstallment = 0,
                InterestInstallment = 0,
                PrincipalBalance = requestModel.LoanAmount,
                Fees = startingFees,
                CashFlow = requestModel.LoanAmount - startingFees
            };

            installments.Add(0, zero);

            for (var i = 1; i <= requestModel.Period; i++)
            {
                var currentPrincipalBalance = CalcHelpers.CalculatePrincipalBalance(installments[i - 1]);
                var interestInstallment = CalcHelpers.CalculateInterestInstallment(requestModel.Interest, currentPrincipalBalance);
                var currentPrincipalInstallment = i == requestModel.Period ? lastPrincipalInstallment : principalInstallment;
                var monthlyInstallment = interestInstallment + currentPrincipalInstallment;
                var fees = CalcHelpers.CalculateFeesCost(i, currentPrincipalBalance, requestModel.Fees);

                installments.Add(i, new InstallmentForRepaymentPlanModel
                {
                    Id = i,
                    Date = DateTime.UtcNow.Date.AddDays(i),
                    MonthlyInstallment = monthlyInstallment,
                    PrincipalInstallment = currentPrincipalInstallment,
                    InterestInstallment = interestInstallment,
                    PrincipalBalance = currentPrincipalBalance,
                    Fees = fees,
                    CashFlow = - monthlyInstallment - fees
                });
            }

            return installments.Select(x => x.Value);

            throw new NotImplementedException();
        }

        private IEnumerable<InstallmentForRepaymentPlanModel> GetAnuityPlan(NewLoanRequestModel requestModel)
        {
            var pmt = CalcHelpers.CalculatePMT(requestModel.Interest, requestModel.Period, requestModel.LoanAmount);

            var installments = new Dictionary<int, InstallmentForRepaymentPlanModel>();
            var startingFees = CalcHelpers.CalculateStartingFeesCost(requestModel.LoanAmount, requestModel.Fees);
            var zero = new InstallmentForRepaymentPlanModel
            {
                Id = 0,
                Date = DateTime.UtcNow.Date,
                MonthlyInstallment = 0,
                PrincipalInstallment = 0,
                InterestInstallment = 0,
                PrincipalBalance = requestModel.LoanAmount,
                Fees = startingFees,
                CashFlow = requestModel.LoanAmount - startingFees
            };

            installments.Add(0, zero);

            for (var i = 1; i <= requestModel.Period; i++)
            {
                var currentPrincipalBalance = CalcHelpers.CalculatePrincipalBalance(installments[i - 1]);
                var interestInstallment = CalcHelpers.CalculateInterestInstallment(requestModel.Interest, currentPrincipalBalance);
                var fees = CalcHelpers.CalculateFeesCost(i, currentPrincipalBalance, requestModel.Fees);
                var principalInstallment = i == requestModel.Period ? requestModel.LoanAmount - installments.Sum(x => x.Value.PrincipalInstallment) : pmt - interestInstallment;
                var monthlyInstallment = i == requestModel.Period ? principalInstallment + interestInstallment : pmt;
                installments.Add(i, new InstallmentForRepaymentPlanModel
                {
                    Id = i,
                    Date = DateTime.UtcNow.Date.AddDays(i),
                    MonthlyInstallment = monthlyInstallment,
                    PrincipalInstallment = principalInstallment,
                    InterestInstallment = interestInstallment,
                    PrincipalBalance = currentPrincipalBalance,
                    Fees = fees,
                    CashFlow = - monthlyInstallment - fees
                });
            }

            return installments.Select(x => x.Value);
        }
    }
}

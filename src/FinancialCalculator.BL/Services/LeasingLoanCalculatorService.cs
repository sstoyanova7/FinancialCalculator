namespace FinancialCalculator.Services
{
    using FinancialCalculator.BL.Utilities;
    using FinancialCalculator.Models.RequestModels;
    using FinancialCalculator.Models.ResponseModels;
    using System;
    using Serilog;
    using System.Net;
    using FinancialCalculator.BL.Validation;
    using Microsoft.AspNetCore.Http;

    public class LeasingLoanCalculatorService : ICalculatorService<LeasingLoanResponseModel, LeasingLoanRequestModel>
    {
        private readonly ILogger _logger;
        private IValidator<LeasingLoanRequestModel> _leasingLoanValidator;
        private readonly IJWTService _jWTService;
        private readonly IRequestHistoryDataService _requestHistoryDataService;

        public LeasingLoanCalculatorService(ILogger logger,
            IValidator<LeasingLoanRequestModel> leasingLoanValidator,
            IJWTService jWTService,
            IRequestHistoryDataService requestHistoryDataService)
        {
            _logger = logger.ForContext<LeasingLoanCalculatorService>();
            _leasingLoanValidator = leasingLoanValidator;
            _jWTService = jWTService;
            _requestHistoryDataService = requestHistoryDataService;
        }

        public LeasingLoanResponseModel Calculate(LeasingLoanRequestModel requestModel, string cookieValue)
        {
            RequestHistoryResponseModel requestHistory = null;
            if (cookieValue != null)
            {
                UserRequestModel user = _jWTService.decodeJWTAsync(cookieValue).Result;
                requestHistory = new RequestHistoryResponseModel();
                requestHistory.Request_Time = DateTime.Now;
                requestHistory.Calculator_Type = "leasing";
                requestHistory.User = user;
            }

            var validated = _leasingLoanValidator.Validate(requestModel);
            if (!validated.IsValid)
            {
                _logger.Error($"Leasing Loan BadRequest! {validated}");

                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.BadRequest,
                    ErrorMessage = validated.ToString()
                };
            }

            //TO DO: AnnualPercentCost how to calculate ?
            try
            {
                var totalFees = CalcHelpers.GetFeeCost(requestModel.StartingFee, requestModel.ProductPrice);
                var totalCost = totalFees + requestModel.StartingInstallment + requestModel.Period * requestModel.MonthlyInstallment;

                if (totalCost < requestModel.ProductPrice)
                {
                    return new LeasingLoanResponseModel
                    {
                        Status = HttpStatusCode.BadRequest,
                        ErrorMessage = "You cannot have a leasing loan with these parameters."
                    };
                }

                LeasingLoanResponseModel leasingLoanResponse = new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.OK,
                    //AnnualPercentCost
                    TotalCost = totalCost,
                    TotalFees = totalFees
                };

                if (requestHistory != null)
                {
                    requestHistory.Calculation_Result = "Input: " + requestModel.ToString().Trim() + " Result: " + leasingLoanResponse.ToString().Trim();
                    requestHistory.User_Agent = requestModel.UserAgent;
                    _requestHistoryDataService.insertRequest(requestHistory);
                }

                return leasingLoanResponse;
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occured while trying to calculate leasing loan.\nInput: {requestModel}\nMessage: {ex.Message}");

                return new LeasingLoanResponseModel
                {
                    Status = HttpStatusCode.InternalServerError,
                    ErrorMessage = "Something went wrong. Please contact our support team."
                };
            }
        }
    }
}

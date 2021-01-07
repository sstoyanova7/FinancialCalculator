using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using FinancialCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace FinancialCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialCalculatorController : ControllerBase
    {
        private readonly ILogger<FinancialCalculatorController> _logger;
        private readonly ICalculatorService _service;

        public FinancialCalculatorController(ILogger<FinancialCalculatorController> logger, ICalculatorService service)
        {
            _logger = logger;
            _service = service;
        }


        [HttpPost] //should it be post?
        [Route("api/calculateNewLoan")]
        public NewLoanResponseModel CalculateNewLoan([FromBody] NewLoanRequestModel requestModel)
        {
            return _service.CalculateNewLoan(requestModel);
        }


        [HttpPost] //should it be post?
        [Route("api/calculateRefinancingLoan")]
        public RefinancingLoanResponseModel CalculateRefinancingLoan([FromBody] RefinancingLoanRequestModel requestModel)
        {
            return new RefinancingLoanResponseModel();
        }

        [HttpPost] //should it be post?
        [Route("api/calculateLeasingLoan")]
        public LeasingLoanResponseModel CalculateLeasingLoan([FromBody] LeasingLoanRequestModel requestModel)
        {
            return new LeasingLoanResponseModel();
        }
    }
}

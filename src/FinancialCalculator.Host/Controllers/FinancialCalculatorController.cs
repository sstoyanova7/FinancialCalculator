namespace FinancialCalculator.Controllers
{
    using Models.RequestModels;
    using Models.ResponseModels;
    using Services;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Serilog;

    [ApiController]
    [Route("[controller]")]
    public class FinancialCalculatorController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICalculatorService _service;

        public FinancialCalculatorController(ILogger logger, ICalculatorService service)
        {
            _logger = logger.ForContext<FinancialCalculatorController>();
            _service = service;
        }

        [HttpGet]
        [Route("api/test")]
        public void Test()
        {
            _logger.Information("Test controller is called from {caller}", HttpContext.Request.Headers["Hi"]);
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
            return _service.CalculateLeasingLoan(requestModel);
        }
    }
}

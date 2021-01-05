using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinancialCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialCalculatorController : ControllerBase
    {
        private readonly ILogger<FinancialCalculatorController> _logger;

        public FinancialCalculatorController(ILogger<FinancialCalculatorController> logger)
        {
            _logger = logger;
        }


        [HttpPost] //should it be post?
        [Route("api/calculateNewLoan")]
        public NewLoanResponseModel CalculateNewLoan([FromBody] NewLoanRequestModel requestModel)
        {
            return new NewLoanResponseModel();
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

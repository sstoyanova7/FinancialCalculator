namespace FinancialCalculator.Controllers
{
    using Models.RequestModels;
    using Models.ResponseModels;
    using Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class FinancialCalculatorController : ControllerBase
    {
        private readonly ICalculatorService _service;

        public FinancialCalculatorController(ICalculatorService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/calculateNewLoan")]
        public NewLoanResponseModel CalculateNewLoan([FromBody] NewLoanRequestModel requestModel)
        {
            return _service.CalculateNewLoan(requestModel);
        }


        [HttpPost]
        [Route("api/calculateRefinancingLoan")]
        public RefinancingLoanResponseModel CalculateRefinancingLoan([FromBody] RefinancingLoanRequestModel requestModel)
        {
            return _service.CalculateRefinancingLoan(requestModel);
        }

        [HttpPost]
        [Route("api/calculateLeasingLoan")]
        public LeasingLoanResponseModel CalculateLeasingLoan([FromBody] LeasingLoanRequestModel requestModel)
        {
            return _service.CalculateLeasingLoan(requestModel);
        }
    }
}

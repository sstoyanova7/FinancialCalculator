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
        private readonly ICalculatorService<NewLoanResponseModel, NewLoanRequestModel> _newLoanService;
        private readonly ICalculatorService<RefinancingLoanResponseModel, RefinancingLoanRequestModel> _refinancingLoanService;
        private readonly ICalculatorService<LeasingLoanResponseModel, LeasingLoanRequestModel> _leasingLoanService;

        public FinancialCalculatorController(
            ICalculatorService<NewLoanResponseModel, NewLoanRequestModel> newLoanService,
            ICalculatorService<RefinancingLoanResponseModel, RefinancingLoanRequestModel> refinancingLoanService,
            ICalculatorService<LeasingLoanResponseModel, LeasingLoanRequestModel> leasingLoanService)
        {
            _newLoanService = newLoanService;
            _refinancingLoanService = refinancingLoanService;
            _leasingLoanService = leasingLoanService;
        }

        [HttpPost]
        [Route("api/calculateNewLoan")]
        public NewLoanResponseModel CalculateNewLoan([FromBody] NewLoanRequestModel requestModel)
        {
            string cookieValue = Request.Cookies["Auth-Tst"];
            return _newLoanService.Calculate(requestModel, cookieValue);
        }


        [HttpPost]
        [Route("api/calculateRefinancingLoan")]
        public RefinancingLoanResponseModel CalculateRefinancingLoan([FromBody] RefinancingLoanRequestModel requestModel)
        {
            string cookieValue = Request.Cookies["Auth-Tst"];
            return _refinancingLoanService.Calculate(requestModel, cookieValue);
        }

        [HttpPost]
        [Route("api/calculateLeasingLoan")]
        public LeasingLoanResponseModel CalculateLeasingLoan([FromBody] LeasingLoanRequestModel requestModel)
        {
            string cookieValue = Request.Cookies["Auth-Tst"];
            return _leasingLoanService.Calculate(requestModel, cookieValue);
        }
    }
}

using FinancialCalculator.Models.ResponseModels;
using FinancialCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialCalculator.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class RequestHistoryController : ControllerBase
    {
        private readonly IRequestHistoryDataService requestHistoryDataService;

        public RequestHistoryController(IRequestHistoryDataService service)
        {
            requestHistoryDataService = service;
        }

        [HttpGet]
        [Route("")]
        public List<RequestHistoryResponseModel> GetAllRequestHistory([FromQuery(Name = "type")] string type)
        {
            string cookieValue = Request.Cookies["Auth-Tst"];
            return requestHistoryDataService.getAllRequestsByType(type, cookieValue).Result;
        }
    }
}

using FinancialCalculator.Models.ResponseModels;
using System.Collections.Generic;
using System.Net;

namespace FinancialCalculator.Models.RequestModels
{
    public class UserRequestModel
    {
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<RequestHistoryResponseModel> requestHistories { get; set; }
    }
}

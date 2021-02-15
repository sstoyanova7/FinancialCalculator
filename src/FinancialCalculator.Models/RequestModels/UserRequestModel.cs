using FinancialCalculator.Models.ResponseModels;
using System.Collections.Generic;

namespace FinancialCalculator.Models.RequestModels
{
    public class UserRequestModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<RequestHistoryResponseModel> requestHistories { get; set; }
    }
}

using FinancialCalculator.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialCalculator.Services
{
    public interface IRequestHistoryDataService
    {
        Task<List<RequestHistoryResponseModel>> getAllRequestsByType(string type, string cookieValue);
        void insertRequest(RequestHistoryResponseModel request);
        Task<RequestHistoryResponseModel> getRequestById(long id);
        void SetRequestHistoryValues(RequestHistoryResponseModel requestHistory, string cookieValue, string calculatorType);
    }
}

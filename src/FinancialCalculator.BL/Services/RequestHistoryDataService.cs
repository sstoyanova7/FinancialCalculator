using Dapper;
using FinancialCalculator.BL.Configuration.Database;
using FinancialCalculator.Models.RequestModels;
using FinancialCalculator.Models.ResponseModels;
using FinancialCalculator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialCalculator.BL.Services
{
    public class RequestHistoryDataService : IRequestHistoryDataService
    {
        private readonly IDatabaseConnectionFactory _database;
        private readonly IJWTService _jWTService;

        public RequestHistoryDataService(IDatabaseConnectionFactory database, IJWTService jWTService)
        {
            _database = database;
            _jWTService = jWTService;
        }

        public async Task<List<RequestHistoryResponseModel>> getAllRequestsByType(string type, string cookieValue)
        {
            UserRequestModel user = _jWTService.decodeJWTAsync(cookieValue).Result;
            using var conn = await _database.CreateConnectionAsync();
            var sql = "select * from [User] inner join [Request_History] on [User].Id = [Request_History].User_Id where [User].Id = " + user.Id + " and [Request_History].Calculator_Type = '" + type + "'";
            var requestHistories = await conn.QueryAsync<UserRequestModel, RequestHistoryResponseModel, RequestHistoryResponseModel>(sql, (user, requestHistory) => {
                requestHistory.User = user;
                return requestHistory;
            },
            splitOn: "Id");

            return (List<RequestHistoryResponseModel>)requestHistories;
        }

        public async void insertRequest(RequestHistoryResponseModel request)
        {
            using var conn = await _database.CreateConnectionAsync();

            var parameters = new
            {
                User_Id = request.User.Id,
                User_Agent = request.User_Agent,
                Calculation_Result = request.Calculation_Result,
                Calculator_Type = request.Calculator_Type,
                Request_Time = DateTime.Now
            };
            
            await conn.ExecuteAsync("INSERT INTO [Request_History](User_Id, User_Agent, Calculation_Result, Calculator_Type, Request_Time) " +
                "VALUES (@User_Id, @User_Agent, @Calculation_Result, @Calculator_Type, @Request_Time)", parameters);
        }

        public async Task<RequestHistoryResponseModel> getRequestById(long id)
        {
            using var conn = await _database.CreateConnectionAsync();
            var parameters = new { Id = id };
            var result = conn.QueryFirstOrDefault<RequestHistoryResponseModel>
               ("SELECT * FROM [Request_History] WHERE Id = @Id", parameters);
            return result;
        }

        public void SetRequestHistoryValues(RequestHistoryResponseModel requestHistory, string cookieValue, string calculatorType)
        {
            if (cookieValue != null)
            {
                UserRequestModel user = _jWTService.decodeJWTAsync(cookieValue).Result;
                requestHistory = new RequestHistoryResponseModel();
                requestHistory.Request_Time = DateTime.Now;
                requestHistory.Calculator_Type = calculatorType;
                requestHistory.User = user;
            }
        }

    }
}

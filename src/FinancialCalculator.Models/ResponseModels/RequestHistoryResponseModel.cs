using FinancialCalculator.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialCalculator.Models.ResponseModels
{
    public class RequestHistoryResponseModel
    {
        public long Id { get; set; }
        public string User_Agent { get; set; }
        public string Calculation_Result { get; set; }
        public string Calculator_Type { get; set; }
        public DateTime Request_Time { get; set; }
        public UserRequestModel User { get; set; }
    }
}

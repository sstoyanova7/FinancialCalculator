using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialCalculator.Models.RequestModels
{
    public class UserRequestModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}

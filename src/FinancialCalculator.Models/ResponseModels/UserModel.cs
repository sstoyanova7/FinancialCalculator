using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialCalculator.Models.ResponseModels
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

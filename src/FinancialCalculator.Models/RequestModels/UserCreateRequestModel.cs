﻿namespace FinancialCalculator.Models.RequestModels
{
    public class UserCreateRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
    }
}

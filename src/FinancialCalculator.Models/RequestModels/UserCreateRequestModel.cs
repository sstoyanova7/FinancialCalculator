﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialCalculator.Models.RequestModels
{
    public class UserCreateRequestModel
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}

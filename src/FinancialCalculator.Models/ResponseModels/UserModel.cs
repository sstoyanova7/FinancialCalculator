﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialCalculator.Models.ResponseModels
{
    public class UserModel
    {
        public long id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}

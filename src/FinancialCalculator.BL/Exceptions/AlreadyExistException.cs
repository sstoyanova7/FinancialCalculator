using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialCalculator.BL.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException()
        {
        }

        public AlreadyExistException(string message) : base(message)
        { }
    }
}

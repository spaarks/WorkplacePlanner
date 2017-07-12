using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Utills.CustomExceptions
{
    public class InvalidDateRangeException : WorkplacePlannerException
    {
        public InvalidDateRangeException() : base("Start date is greater than the End date.") { }
       
    }
}

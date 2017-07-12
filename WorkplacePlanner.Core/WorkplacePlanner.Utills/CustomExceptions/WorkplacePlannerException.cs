using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Utills.CustomExceptions
{
    public class WorkplacePlannerException : Exception
    {
        private string _customMessage = "Something went wrong. Please check the data submited and try again.";

        public WorkplacePlannerException() { }

        public WorkplacePlannerException(string message)
        {
            _customMessage = message;
        }

        public override string ToString()
        {
            return _customMessage;
        }
    }
}

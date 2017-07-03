using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Utills.CustomExceptions
{
    public class TeamDefaultUsageTypeMissingException : WorkplacePlannerException
    {
        public TeamDefaultUsageTypeMissingException() : base("Cannot locate team's default usage type for the given month.")
        {

        }
    }
}

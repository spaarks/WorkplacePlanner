using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Utills.CustomExceptions
{
    public class AuthenticationException : WorkplacePlannerException
    {
        public AuthenticationException() : base("Username or password is invalid") { }
    }
}

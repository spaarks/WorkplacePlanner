using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Utills.CustomExceptions
{
    public class DuplicateKeyException : WorkplacePlannerException
    {
        public DuplicateKeyException(string value) : base(string.Format("{0} already exists", value)) { }
        public DuplicateKeyException(string table, string column, string value): base (string.Format("{0} {1}: {2} already exists", table, column, value)) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Utills.CustomExceptions
{
    public class RecordNotFoundException : WorkplacePlannerException
    {
        public RecordNotFoundException() : base("Record Not found") { }

        public RecordNotFoundException(string entity, int id) : base(string.Format("{0} having id: {1}  Not found", entity, id)) { }
    }
}

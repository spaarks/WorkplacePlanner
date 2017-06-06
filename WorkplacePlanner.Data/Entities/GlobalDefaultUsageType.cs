using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class GlobalDefaultUsageType : BaseEntity
    {
        public int Id { get; set; }

        [ForeignKey("UsageType")]
        public int UsageTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual UsageType UsageType { get; set; }
    }
}

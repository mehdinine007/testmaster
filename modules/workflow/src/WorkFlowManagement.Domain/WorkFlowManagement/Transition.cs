using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    [Table("Transitions", Schema = "Flow")]
    public class Transition: FullAuditedEntity<int>
    {
        public int ActivitySourceId { get; set; }
        public virtual Activity ActivitySource { get; set; }
        public int ActivityTargetId { get; set; }
        public virtual Activity ActivityTarget { get; set; }
        public int SchemeId { get; set; }
        public virtual Scheme Scheme { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    [Table("Schemes", Schema = "Flow")]
    public class Scheme: FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public bool Status { get; set; }
        public int Priority { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Process> Processes { get; set; }

    }
}

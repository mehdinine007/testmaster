using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    [Table("Activities", Schema = "Flow")]
    public class Activity : FullAuditedEntity<int>
    {

        public string Title { get; set; }
        public int Priority { get; set; }
        public FlowTypeEnum FlowType { get; set; }
        public EntityTypeEnum Entity { get; set; }
        public TypeEnum Type { get; set; }
        public int SchemeId { get; set; }
        public virtual Scheme Scheme { get; set; }

        public virtual ICollection<Transition> SourceTransitions { get; set; }
        public virtual ICollection<Transition> TargetTransitions { get; set; }
        public virtual ICollection<ActivityRole> ActivityRoles { get; set; }
        public virtual ICollection<Process> Processes { get; set; }
        public virtual ICollection<Process> PreviousProcesses { get; set; }

       




    }
}

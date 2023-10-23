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
    [Table("Inbox", Schema = "Flow")]
    public class Inbox:FullAuditedEntity<int>
    {

        public Guid ProcessId { get; set; }
        public virtual Process Process { get; set; }

        public Guid PersonId { get; set; }
        public virtual Person Person { get; protected set; }
        public int OrganizationChartId { get; set; }
        public virtual OrganizationChart OrganizationChart { get; set; }

        public int OrganizationPositionId { get; set; }
        public virtual OrganizationPosition OrganizationPosition { get; set; }
        public StateEnum State { get; set; }

        public InboxStatusEnum Status { get; set; }
        public string ReferenceDescription { get; set; }
        [ForeignKey("Inbox")]
        public int? ParentId { get; set; }
        public virtual Inbox Parent { get; set; }

        

    }
}

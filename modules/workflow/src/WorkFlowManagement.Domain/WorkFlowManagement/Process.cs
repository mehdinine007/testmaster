using Nest;
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
    [Table("Processes", Schema = "Flow")]
    public class Process: FullAuditedEntity<Guid>
    {

        public string Title { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public StateEnum State { get; set; }
        public StatusEnum Status { get; set; }
        public int SchemeId { get; set; }
        public virtual Scheme Scheme { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public int? PreviousActivityId { get; set; }
        public virtual Activity PreviousActivity { get; set; }

        public int OrganizationChartId { get; set; }
        public virtual OrganizationChart OrganizationChart { get; set; }

        public int CreatedOrganizationChartId { get; set; }
        public virtual OrganizationChart CreatedOrganizationChart { get; set; }
        public int? PreviousOrganizationChartId { get; set; }
        public virtual OrganizationChart PreviousOrganizationChart { get; set; }

        public Guid PersonId { get; set; }
        public Guid? PreviousPersonId { get; set; }
        public Guid CreatedPersonId { get; set; }







    }
}

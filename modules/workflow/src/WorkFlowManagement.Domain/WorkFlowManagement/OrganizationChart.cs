#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    public class OrganizationChart : FullAuditedEntity<int>
    {
        public OrganizationChart()
        {
            Childrens = new HashSet<OrganizationChart>();
        }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public virtual OrganizationChart Parent { get; set; }
        public virtual ICollection<OrganizationChart> Childrens { get; set; }
        public virtual ICollection<OrganizationPosition> OrganizationPositions { get; set; }
        public OrganizationTypeEnum OrganizationType { get; set; }
        public virtual ICollection<RoleOrganizationChart> RoleOrganizationCharts { get; set; }
        public virtual ICollection<Process> Processes { get; set; }
        public virtual ICollection<Process> PreviousProcesses { get; set; }
        public virtual ICollection<Process> CreatedProcesses { get; set; }
        public virtual ICollection<Inbox> Inboxes { get; set; }


    }
}

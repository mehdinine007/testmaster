using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

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
    }
}

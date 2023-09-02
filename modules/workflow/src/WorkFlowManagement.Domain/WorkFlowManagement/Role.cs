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
    [Table("Roles", Schema = "Flow")]
    public class Role : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public bool Status { get; set; }
        public string Security { get; set; }
        public virtual ICollection<RoleOrganizationChart> RoleOrganizationCharts { get; set; }
    }
}

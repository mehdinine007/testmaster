using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    [Table("RoleChart", Schema = "Flow")]
    public class WorkFlowRoleChart : FullAuditedEntity<int>
    {
        public int WorkFlowRoleId { get; set; }
        public int OrganizationChartId { get; set; }
        public virtual WorkFlowRole WorkFlowRole { get; set; } 
        public virtual OrganizationChart OrganizationChart { get; set; } 
    }
}

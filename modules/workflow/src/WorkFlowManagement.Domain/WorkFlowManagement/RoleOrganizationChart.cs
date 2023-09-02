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
    [Table("RoleOrganizationCharts", Schema = "Flow")]
    public class RoleOrganizationChart : FullAuditedEntity<int>
    {
        public int RoleId { get; set; }
        public int OrganizationChartId { get; set; }
        public virtual Role Role { get; set; } 
        public virtual OrganizationChart OrganizationChart { get; set; } 
    }
}

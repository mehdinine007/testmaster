using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ReportManagement.Domain.ReportManagement
{
    public class Dashboard : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public int Priority { get; set; }
        public virtual ICollection<DashboardWidget> DashboardWidgets { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ReportManagement.Domain.ReportManagement
{
    public class DashboardWidget: FullAuditedEntity<int>
    {
        public int DashboardId { get; set; }
        public virtual Dashboard Dashboard { get; set; }
        public int WidgetId { get; set; }
        public virtual Widget Widget { get; set; }

    }
}

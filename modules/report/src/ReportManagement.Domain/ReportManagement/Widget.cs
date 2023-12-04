using ReportManagement.Domain.Shared.ReportManagement.Dtos;
using ReportManagement.Domain.Shared.ReportManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ReportManagement.Domain.ReportManagement
{
    public class Widget :FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public WidgetTypeEnum Type { get; set; }
        public string Command { get; set; }
        public string Fields { get; set; }
        public string Condition { get; set; }

        public virtual ICollection<Dashboard> Dashboards { get; set; }

        public OutPutTypeEnum OutPutType { get; set; }
        public string Roles { get; set; }

    }
}

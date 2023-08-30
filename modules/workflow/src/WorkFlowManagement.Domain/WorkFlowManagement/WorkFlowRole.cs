using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    public class WorkFlowRole : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public bool Status { get; set; }
        public string Security { get; set; }
    }
}

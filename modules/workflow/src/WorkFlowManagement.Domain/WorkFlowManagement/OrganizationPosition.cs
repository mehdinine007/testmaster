﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace WorkFlowManagement.Domain.WorkFlowManagement
{
    public class OrganizationPosition:FullAuditedEntity<int>
    {
        public int OrganizationChartId { get; set; }
        public Guid PersonId { get; set; }
        public virtual Person Person { get; protected set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
        public virtual OrganizationChart OrganizationChart { get; protected set; }
        public virtual ICollection<Inbox> Inboxes { get; set; }
        public virtual ICollection<Process> Processes { get; set; }




    }
}

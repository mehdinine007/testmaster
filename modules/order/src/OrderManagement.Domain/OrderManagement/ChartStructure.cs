using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class ChartStructure : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public ChartTypeEnum Type { get; set; }
        public string Categories { get; set; }
        public string Series { get; set; }
        public int Priority { get; set; }

    }
}

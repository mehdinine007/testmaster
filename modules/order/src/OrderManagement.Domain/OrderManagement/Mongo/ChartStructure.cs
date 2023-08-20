using MongoDB.Bson;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class ChartStructure : FullAuditedEntity<ObjectId>
    {
        public string Title { get; set; }
        public ChartTypeEnum Type { get; set; }
        public List<string> Categories { get; set; }
        public ChartSeries Series { get; set; }
        public int Priority { get; set; }
    }
}

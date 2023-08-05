using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class PropertyCategory : FullAuditedEntity<ObjectId>
    {
        public string Title { get; set; }
        public List<Property> Properties { get; set; }
        public int Priority { get; set; }
    }
}

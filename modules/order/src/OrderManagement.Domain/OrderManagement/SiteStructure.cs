using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class SiteStructure : FullAuditedEntity<int>
    {
        public int Code { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        public SiteStructureTypeEnum Type { get; set; }
        public string Description { get; set; }
    }
}

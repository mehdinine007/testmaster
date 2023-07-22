using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class Attachment  : FullAuditedEntity<Guid>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string FileExtension { get; set; }
        public AttachmentEntityEnum Entity { get; set; }
        public int EntityId { get; set; }
        public AttachmentEntityTypeEnum EntityType { get; set; }
    }
}

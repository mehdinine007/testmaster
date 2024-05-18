using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderManagement.Domain.Shared.OrderManagement.Enums;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FileExtension { get; set; }
        public AttachmentEntityEnum Entity { get; set; }
        public int EntityId { get; set; }
        public AttachmentEntityTypeEnum EntityType { get; set; }
        public AttachmentLocationEnum Location { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int Priority { get; set; }
        public DeviceEnum Device { get; set; }
        public int VersionNumber { get; set; }
    }
}

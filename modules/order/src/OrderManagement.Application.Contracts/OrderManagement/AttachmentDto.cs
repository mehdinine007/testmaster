using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public AttachmentEntityEnum Entity { get; set; }
        public int EntityId { get; set; }
        public AttachmentEntityTypeEnum EntityType { get; set; }

    }
}

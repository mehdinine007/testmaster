using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class AttachFileDto  
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public AttachmentEntityEnum Entity { get; set; }
        public int EntityId { get; set; }
        public AttachmentEntityTypeEnum EntityType { get; set; }
        public AttachmentLocationEnum Location { get; set; }
        public IFormFile File { get; set; }
    }
}

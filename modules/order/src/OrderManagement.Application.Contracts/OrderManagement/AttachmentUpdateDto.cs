using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentUpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IFormFile? File { get; set; }
        public AttachmentEntityTypeEnum Type { get; set; }
        public AttachmentLocationEnum Location { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public List<string>? Content { get; set; }

    }
}

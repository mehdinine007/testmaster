using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class UploadFileDto
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public AttachmentEntityTypeEnum Type { get; set; }
        public AttachmentLocationEnum Location { get; set; }
    }
}

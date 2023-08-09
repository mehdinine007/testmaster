using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class UploadFileDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IFormFile File { get; set; }
        public AttachmentEntityTypeEnum Type { get; set; }
        public AttachmentLocationEnum Location { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public List<string>? Content { get; set; }
    }
}

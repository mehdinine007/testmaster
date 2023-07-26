using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class UploadFileDto
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public AttachmentEntityTypeEnum AttachmentEntityTypeEnum { get; set; }
    }
}

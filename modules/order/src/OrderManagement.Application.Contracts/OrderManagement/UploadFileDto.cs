using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class UploadFileDto
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public AttachmentEntityTypeEnum AttachmentEntityTypeEnum { get; set; }
    }
}

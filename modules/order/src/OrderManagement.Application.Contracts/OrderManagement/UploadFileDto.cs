using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class UploadFileDto : AttachmentDto
    {
        public IFormFile File { get; set; }
    }
}

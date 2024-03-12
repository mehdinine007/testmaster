using OrderManagement.Domain;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class CustomerOrderQueryDto
    {
        public List<AttachmentEntityTypeEnum> AttachmentType { get; set; }
        public List<AttachmentLocationEnum> Attachmentlocation { get; set; }
    }
}

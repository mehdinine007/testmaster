using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class OrganizationQueryDto
    {
        public bool? IsActive { get; set; }
        public List<AttachmentEntityTypeEnum> AttachmentEntityType { get; set; }
        public List<AttachmentLocationEnum> AttachmentLocation { get; set; }

    }
}

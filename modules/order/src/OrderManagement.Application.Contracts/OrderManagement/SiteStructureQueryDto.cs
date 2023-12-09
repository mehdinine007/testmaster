using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class SiteStructureQueryDto
    {
        public string? AttachmentType { get; set; }
        public string? AttachmentLocation { get; set; }
        public int? Location { get; set; }
    }
}

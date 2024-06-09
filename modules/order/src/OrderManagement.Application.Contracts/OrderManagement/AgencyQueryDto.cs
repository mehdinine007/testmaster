using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AgencyQueryDto
    {
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public AgencyTypeEnum? AgencyType { get; set; }
        public List<AttachmentEntityTypeEnum> AttachmentEntityType { get; set; }
        public List<AttachmentLocationEnum> AttachmentLocation { get; set; }
       
    }
}

using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AgencyCreateDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool Visible { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public AgencyTypeEnum AgencyType { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
      
    }
}

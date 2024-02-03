using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AdvertisementDto
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public List<AdvertisementDetailDto> AdvertisementDetails { get; set; }

    }
}

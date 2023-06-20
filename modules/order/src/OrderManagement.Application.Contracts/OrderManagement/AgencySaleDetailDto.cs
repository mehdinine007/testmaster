using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AgencySaleDetailDto
    {
        public int Id { get; set; }
        public int DistributionCapacity { get; set; }
        public int ReserveCount { get; set; }
        public int AgencyId { get; set; }
      
        public int SaleDetailId { get; set; }
      
    }
}

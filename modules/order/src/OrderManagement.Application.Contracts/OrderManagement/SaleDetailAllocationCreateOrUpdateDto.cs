using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class SaleDetailAllocationCreateOrUpdateDto
    {
        public int Id { get; set; }
        public int SaleDetailId { get; set; }
        public int Count { get; set; }
        public bool IsComplete { get; set; }
        public int? TotalCount { get; set; }
        public int? SeasonAllocationId { get; set; }

    }
}

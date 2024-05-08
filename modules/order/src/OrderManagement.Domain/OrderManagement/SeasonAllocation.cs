using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class SeasonAllocation: FullAuditedEntity<int>
    {

        public int Code { get; set; }
        public string Title { get; set; }
        public SeasonTypeEnum SeasonId { get; set; }
        public int Year { get; set; }
        public ICollection<CustomerOrder> CustomerOrders { get; set; }

    }
}

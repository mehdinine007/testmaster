using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class SaleDetailListDto
    {
        public int EsaleTypeId { get; set; }
        public string EsaleTypeName { get; set; }
        public decimal CarFee { get; set; }
        public decimal MinimumAmountOfProxyDeposit { get; set; }
        public Guid UID { get; set; }

    }
}

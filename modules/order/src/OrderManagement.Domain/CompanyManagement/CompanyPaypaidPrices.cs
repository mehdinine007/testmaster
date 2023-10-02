using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.CompanyManagement
{
    public class CompanyPaypaidPrices : FullAuditedEntity<long>
    {
        public DateTime TranDate { get; set; }
        public long PayedPrice { get; set; }
        public long ClientsOrderDetailByCompanyId { get; set; }
        public virtual ClientsOrderDetailByCompany ClientsOrderDetailByCompany { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.CompanyManagement
{
    public class CompanySaleCallDates: FullAuditedEntity<long>
    {
        public DateTime StartTurnDate { get; set; }
        public DateTime EndTurnDate { get; set; }
        public long ClientsOrderDetailByCompanyId { get; set; }
        public virtual ClientsOrderDetailByCompany ClientsOrderDetailByCompany { get; set; }
    }
}

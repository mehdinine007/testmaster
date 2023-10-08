using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.CompanyManagement
{
    public class CompanyProduction: FullAuditedEntity<long>
    {
        public string CarCode { get; set; }
        public string CarDesc { get; set; }
        public DateTime ProductionDate { get; set; }
        public int ProductionCount { get; set; }
    }
}

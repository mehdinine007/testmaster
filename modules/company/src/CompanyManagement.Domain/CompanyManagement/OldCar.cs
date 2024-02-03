using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CompanyManagement.Domain.CompanyManagement
{
    public class OldCar : FullAuditedEntity<int>
    {
        public string Vehicle { get; set; }
        public string Nationalcode { get; set; }
        public string Vin { get; set; }
        public string ChassiNo { get; set; }
        public string EngineNo { get; set; }
        public int BatchNo { get; set; }


    }
}

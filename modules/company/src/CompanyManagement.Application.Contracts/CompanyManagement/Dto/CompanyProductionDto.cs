using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CompanyManagement.Application.Contracts.CompanyManagement
{
    public class CompanyProductionDto : Entity<long>
    {
        public string CarCode { get; set; }
        public string CarDesc { get; set; }
        public DateTime ProductionDate { get; set; }
        public int ProductionCount { get; set; }
    }
}

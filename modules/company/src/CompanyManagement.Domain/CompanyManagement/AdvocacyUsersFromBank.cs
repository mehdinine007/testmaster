using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CompanyManagement.Domain.CompanyManagement
{
    public class AdvocacyUsersFromBank : FullAuditedEntity<int>
    {
        public string nationalcode { get; set; }
        public string bankName { get; set; }
        public decimal price { get; set; }
        public DateTime? dateTime { get; set; }
        public string accountNumber { get; set; }
        public string shabaNumber { get; set; }
        public long UserId { get; set; }
        public Guid UserUid { get; set; }
        public int? BanksId { get; set; }
        public int? CompanyId { get; set; }

    }
}

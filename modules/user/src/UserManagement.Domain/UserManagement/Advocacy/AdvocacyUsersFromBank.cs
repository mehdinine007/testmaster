using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Domain.UserManagement.Bases;

namespace UserManagement.Domain.UserManagement.Advocacy
{
    public class AdvocacyUsersFromBank: FullAuditedEntity<int>
    {
        public string nationalcode { get; set; }
        public string bankName { get; set; }
        public decimal price { get; set; }
        public DateTime? dateTime { get; set; }
        public string accountNumber { get; set; }
        public string shabaNumber { get; set; }
        public long UserId { get; set; }
        public int? BanksId { get; set; }
        public virtual bank Banks { get; set; }

        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}

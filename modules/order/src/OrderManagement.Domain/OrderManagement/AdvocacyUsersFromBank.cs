using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
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
        public int? BanksId { get; set; }
        public virtual Bank Bank { get; set; }
        public int? CompanyId { get; set; }
    }
}

using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.Advocacy
{
    public class AdvocacyUsersFromBank : FullAuditedEntity<int>
    {
        public string nationalcode { get; set; }
        public string bankName { get; set; }
        public decimal price { get; set; }
        public DateTime? dateTime { get; set; }
        public string accountNumber { get; set; }
        public string shabaNumber { get; set; }
        public Guid UserId { get; set; }
        public int? BanksId { get; set; }
        public int? CompanyId { get; set; }

    }
}

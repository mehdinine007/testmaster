using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.Bases
{
    public class CompanyPaypaidPrices: FullAuditedEntity<long>
    {
        public DateTime? TranDate { get; set; }
        public long? PayedPrice { get; set; }
        public long? ClientsOrderDetailByCompanyId { get; set; }
    }
}

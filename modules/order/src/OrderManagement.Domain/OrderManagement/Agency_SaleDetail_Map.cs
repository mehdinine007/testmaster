using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class AgencySaleDetail : FullAuditedEntity<int>
    {
        public int DistributionCapacity { get; set; }

        public int AgencyId { get; set; }

        public int SaleDetailId { get; set; }

        public virtual Agency Agency { get; set; }

        public virtual SaleDetail SaleDetail { get; set; }
    }
}

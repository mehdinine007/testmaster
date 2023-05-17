using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class SaleSchema : FullAuditedEntity<int>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int SaleStatus { get; set; }

    }
}

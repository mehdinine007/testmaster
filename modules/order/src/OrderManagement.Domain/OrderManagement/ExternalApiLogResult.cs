using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class ExternalApiLogResult : FullAuditedEntity<int>
    {
        public string ServiceName { get; set; }
        public string Body { get; set; }
        public string NationalCode { get; set; }
        public string Resopnse { get; set; }
        public int OrderId { get; set; }
        public int SaleId { get; set; }

    }
}

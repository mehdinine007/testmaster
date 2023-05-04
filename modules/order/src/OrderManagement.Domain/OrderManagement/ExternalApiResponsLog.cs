using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class ExternalApiResponsLog : FullAuditedEntity<int>
    {

        public string ServiceName { get; set; }
        public bool Result { get; set; }
        public string NationalCode { get; set; }
        public int OrderId { get; set; }
        public int SaleId { get; set; }
    }
}

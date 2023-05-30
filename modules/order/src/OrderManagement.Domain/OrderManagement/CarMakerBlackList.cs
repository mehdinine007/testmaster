using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class CarMakerBlackList : FullAuditedEntity<long>
    {
        public string Nationalcode { get; set; }
        public string Title { get; set; }
        public int EsaleTypeId { get; set; }
        public int? CarMaker { get; set; }
        public int SaleId { get; set; }
    }
}

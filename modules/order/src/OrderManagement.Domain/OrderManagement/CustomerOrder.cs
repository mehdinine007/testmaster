using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class CustomerOrder: FullAuditedEntity<int>
    {
        public int SaleDetailId { get; set; }
        public long UserId { get; set; }
        public int SaleId { get; set; }
        public PriorityEnum? PriorityId { get; set; }
        public OrderStatusType OrderStatus { get; set; }
        public string DeliveryDateDescription { get; set; }
        public OrderRejectionType? OrderRejectionStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        //TODO: check sale detail is in context or not
        //public SaleDetail SaleDetail { get; set; }
        public int? PriorityUser { get; set; }
    }
}

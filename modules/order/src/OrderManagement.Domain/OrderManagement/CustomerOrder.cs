using OrderManagement.Domain.Shared;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class CustomerOrder : FullAuditedEntity<int>
    {
        public int SaleDetailId { get; set; }
        public long OldUserId { get; set; }
        public Guid UserId { get; set; }
        public int SaleId { get; set; }
        public PriorityEnum? PriorityId { get; set; }
        public OrderStatusType OrderStatus { get; set; }
        public string DeliveryDateDescription { get; set; }
        public OrderRejectionType? OrderRejectionStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public virtual SaleDetail SaleDetail { get; set; }
        public int? PriorityUser { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string ChassiNo { get; set; }
        public string Vehicle { get; set; }
        public int? PspId { get; set; }
        public int AgencyId { get; set; }
        public int? PaymentId { get; set; }
        public int PaymentSecret { get; set; }
        public OrderDeliveryStatusType? OrderDeliveryStatus { get; set; }
        public string OrderDeliveryStatusDesc { get; set; }
        public DateTime? PrioritizationDate { get; set; }
        public DateTime? VehicleSelectDate { get; set; }
        public DateTime? SendToManufacturerDate { get; set; }
        public DateTime? OrderRejectionDate { get; set; }
        public string TrackingCode { get; set; }
    }
}

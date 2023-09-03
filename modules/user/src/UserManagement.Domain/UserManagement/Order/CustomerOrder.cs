using Abp.Domain.Entities.Auditing;
using System;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Sale;

namespace UserManagement.Domain.UserManagement.Order
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
        public virtual User User { get; set; }
        public virtual SaleDetail SaleDetail { get; set; }
        public int? PriorityUser { get; set; }
        
    }
}

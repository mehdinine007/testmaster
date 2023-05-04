using OrderManagement.Domain.Shared;
using System;

namespace OrderManagement.Application.Contracts
{
    public class CustomerOrderDto
    {
        public int Id { get; set; }
        public int SaleDetailId { get; set; }
        public long UserId { get; set; }
        public PriorityEnum? PriorityId { get; set; }
        public string OrderStatus { get; set; }
        public int OrderStatusCode { get; set; }
        public int SaleId { get; set; }
        public string OrderStatusDescription { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
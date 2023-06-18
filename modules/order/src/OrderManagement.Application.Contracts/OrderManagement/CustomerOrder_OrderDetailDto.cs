using System;

namespace OrderManagement.Application.Contracts
{
    public class CustomerOrder_OrderDetailDto
    {
        public DateTime CarDeliverDate { get; set; }

        public string SaleDescription { get; set; }

        public string CompanyName { get; set; }

        public string CarFamilyTitle { get; set; }

        public string CarTypeTitle { get; set; }

        public string CarTipTitle { get; set; }

        public long UserId { get; set; }

        public string OrderstatusTitle { get; set; }

        public DateTime CreationTime { get; set; }

        public int OrderId { get; set; }

        public int OrderStatusCode { get; set; }

        public int? PriorityId { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string DeliveryDateDescription { get; set; }

        public int? OrderRejectionCode { get; set; }

        public string OrderRejectionTitle { get; set; }

        public bool Cancelable { get; set; }
        public int ESaleTypeId { get; set; }
        public DateTime? SalePlanEndDate { get; set; }
    }
}
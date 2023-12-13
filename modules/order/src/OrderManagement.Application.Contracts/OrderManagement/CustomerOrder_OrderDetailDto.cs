using OrderManagement.Application.Contracts.OrderManagement.Models;
using System;
using System.Collections.Generic;

namespace OrderManagement.Application.Contracts
{
    public class CustomerOrder_OrderDetailDto
    {
        public DateTime CarDeliverDate { get; set; }

        public string SaleDescription { get; set; }
        public Guid UserId { get; set; }

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

        public Guid SaleDetailUid { get; set; }

        public string NationalCode { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public DateTime? TransactionCommitDate { get; set; }

        public string TransactionId { get; set; }

        public DateTime ManufactureDate { get; set; }

        public decimal MinimumAmountOfProxyDeposit { get; set; }

        public int? PaymentId { get; set; }

        public DateTime SalePlanEndDate { get; set; }
        public int ProductId { get; set; }

        public ProductAndCategoryViewModel Product { get; set; }

        public string CompanyName { get; set; }
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string TrackingCode { get; set; }

    }
    public class CustomerOrder_OrderDetailTreeDto
    {
        public int? ApproximatePriority { get; set; }

        public int PrimaryPriority { get; set; }

        public List<CustomerOrder_OrderDetailDto> OrderList { get; set; }
    }
}
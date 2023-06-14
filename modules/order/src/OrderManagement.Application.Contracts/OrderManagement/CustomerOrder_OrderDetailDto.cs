using System;
using System.Collections.Generic;

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

        public Guid SaleDetailUid { get; set; }

        public string NationalCode { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public DateTime? TransactionCommitDate { get; set; }

        public string TransactionId { get; set; }

        public DateTime ManufactureDate { get; set; }

        public List<string> CarTipImageUrls { get; set; }

        public decimal MinimumAmountOfProxyDeposit { get; set; }

        public int CarTipId { get; set; }

        public int? PaymentId { get; set; }
    }
}
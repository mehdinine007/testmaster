using OrderManagement.Domain.Shared;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class OrderStatusInquiry : FullAuditedEntity<long>
    {
        public int CompanyId { get; set; }

        public DateTime SubmitDate { get; set; }

        public string InquiryFullResponse { get; set; }

        public int OrderId { get; set; }

        public string ClientNationalCode { get; set; }

        public  OrderDeliveryStatusType? OrderDeliveryStatus { get; set; }

        public virtual ProductAndCategory CompanyCategory { get; protected set; }
    }
}

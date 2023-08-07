using OrderManagement.Domain.Shared;
using System;

namespace OrderManagement.Application.Contracts
{
    public class SaleDetail_Order_InquiryDto
    {
        public int? OrderId { get; set; }

        public Guid? SaleDetailUid { get; set; }
        public AttachmentEntityTypeEnum AttachmentType { get; set; }
       
    }
}
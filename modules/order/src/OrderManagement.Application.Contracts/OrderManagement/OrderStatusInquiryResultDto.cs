using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class OrderStatusInquiryResultDto
    {
        public int CompanyId { get; set; }

        public OrderDeliveryStatusType? OrderDeliveryStatus { get; set; }

        public string OrderDeliveryStatusDescription { get; set; }

        public List<OrderDeliveryStatusViewModel> AvailableDeliveryStatusList { get; set; }

        public DateTime? RejectionDate { get; set; }

        public DateTime? UserRejectionAdvocacyDate { get; set; }
    }

    public class OrderDeliveryStatusViewModel
    {
        public int OrderDeliverySatusCode { get; set; }

        public string Description { get; set; }

        public DateTime? SubmitDate { get; set; }

        public string Title { get; set; }
    }
}
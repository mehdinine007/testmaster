namespace OrderManagement.Application.Contracts
{
    public class OrderStatusInquiryResultDto : OrderStatusInquiryDto
    {
        public string OrderDeliveryStatusDescription { get; set; }

        public List<OrderDeliveryStatusViewModel> AvailableDeliveryStatusList { get; set; }

        public DateTime RejectionDate { get; set; }
    }

    public class OrderDeliveryStatusViewModel
    {
        public int OrderDeliverySatusCode { get; set; }

        public string Description { get; set; }

        public DateTime? SubmitDate { get; set; }

        public string Title { get; set; }
    }
}
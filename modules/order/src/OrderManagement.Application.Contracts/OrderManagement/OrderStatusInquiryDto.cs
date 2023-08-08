namespace OrderManagement.Application.Contracts
{
    public class OrderStatusInquiryDto
    {
        public long Id { get; set; }

        public int CompanyId { get; set; }

        public DateTime SubmitDate { get; set; }

        public string InquiryFullResponse { get; set; }

        public int OrderId { get; set; }

        public string ClientNationalCode { get; set; }

    }
}
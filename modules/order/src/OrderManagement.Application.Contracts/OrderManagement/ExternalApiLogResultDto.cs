namespace OrderManagement.Application.Contracts
{
    public class ExternalApiLogResultDto
    {
        public string ServiceName { get; set; }
        public string Body { get; set; }
        public string NationalCode { get; set; }
        public string Resopnse { get; set; }
        public int OrderId { get; set; }
        public int SaleId { get; set; }
    }
}
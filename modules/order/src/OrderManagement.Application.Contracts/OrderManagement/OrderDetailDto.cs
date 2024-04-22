namespace OrderManagement.Application.Contracts
{
    public class OrderDetailDto
    {
        public Guid UserId { get; set; }

        public long? PaymentPrice { get; set; }

        public string ProductTitle { get; set; }

        public DateTime CreationTime { get; set; }

        public int OrderId { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public DateTime? TransactionCommitDate { get; set; }

        public string TransactionId { get; set; }

        public DateTime BirthDate { get; set; }

        public string IssuingCityTitle { get; set; }

        public string Tel { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string BirthCertId { get; set; }

        public string PspTitle { get; set; }

        public string NationalCode { get; set; }
    }
}
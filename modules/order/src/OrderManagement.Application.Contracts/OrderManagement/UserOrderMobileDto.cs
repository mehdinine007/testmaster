namespace OrderManagement.Application.Contracts
{
    public class UserOrderMobileDto
    {

        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
    }
}
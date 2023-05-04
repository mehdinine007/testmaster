namespace OrderManagement.Application.Contracts
{
    public class CustomerOrderPriorityUserDto
    {

        public int? PriorityUser { get; set; }

        public UserInfoPriorityDto CustomerInformation { get; set; }
    }
}
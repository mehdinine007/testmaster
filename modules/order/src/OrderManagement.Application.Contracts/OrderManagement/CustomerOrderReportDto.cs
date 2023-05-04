using System;

namespace OrderManagement.Application.Contracts
{
    public class CustomerOrderReportDto
    {
        public string CarTipName { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public UsersCustomerOrdersDto CustomerInformation { get; set; }
    }
}
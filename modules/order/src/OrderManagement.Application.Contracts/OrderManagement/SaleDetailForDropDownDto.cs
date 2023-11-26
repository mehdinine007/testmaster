using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class SaleDetailForDropDownDto
    {
        public int Id { get; set; }

        public string SalePlanDescription { get; set; }


        public SaleProcessType SaleProcess { get; set; }

    }
}
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class CreateSaleDetailDto
    {
        public Guid Uid { get; set; } 
        public int Id { get; set; } 
       
        public int SaleId { get; set; }
        public int SalePlanCode { get; set; } // شماره برنامه فروش 

        public string SalePlanDescription { get; set; }

        public DateTime ManufactureDate { get; set; }

        public DateTime SalePlanStartDate { get; set; }

        public DateTime SalePlanEndDate { get; set; }

        public DateTime CarDeliverDate { get; set; }

        public int SaleTypeCapacity { get; set; }

        public double CoOperatingProfitPercentage { get; set; }

        public double RefuseProfitPercentage { get; set; }

        public int EsaleTypeId { get; set; }

        public decimal CarFee { get; set; }

        public decimal MinimumAmountOfProxyDeposit { get; set; }

        public int DeliverDaysCount { get; set; }

        public bool Visible { get; set; }
        public int ColorId { get; set; }
        public int ProductId { get; set; }

        public SaleProcessType SaleProcess { get; set; }

    }
}

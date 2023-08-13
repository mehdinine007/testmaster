using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class SaleDetailListDto
    {
        public int Id { get; set; }
        public Guid UID { get; set; }

        public int CircularSaleCode { get; set; } // شماره بخشنامه فروش

        public int SalePlanCode { get; set; }

        public string SalePlanDescription { get; set; }

        public DateTime ManufactureDate { get; set; }

        public DateTime SalePlanStartDate { get; set; }

        public DateTime SalePlanEndDate { get; set; }

        public DateTime CarDeliverDate { get; set; }

        public int SaleTypeCapacity { get; set; }

        public double CoOperatingProfitPercentage { get; set; }

        public double RefuseProfitPercentage { get; set; }

        public int EsaleTypeId { get; set; }
        public string EsaleTypeName { get; set; }
        public string EsaleName { get; set; }

        public decimal CarFee { get; set; }

        public decimal MinimumAmountOfProxyDeposit { get; set; }

        public int DeliverDaysCount { get; set; }
        public string CompanyImageUrl { get; set; }
        public bool Visible { get; set; }

        public string ColorTitle { get; set; }

        public int ColorId { get; set; }

        public int SaleId { get; set; }

        public string SaleTitle { get; set; }

    }
}

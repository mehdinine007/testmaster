using System;
using System.Collections.Generic;

namespace OrderManagement.Application.Contracts
{
    public class SaleDetailDto
    {
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

        public decimal CarFee { get; set; }

        public decimal MinimumAmountOfProxyDeposit { get; set; }

        public int DeliverDaysCount { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyImageUrl { get; set; }

        public int CarFamilyId { get; set; }

        public string CarFamilyTitle { get; set; }

        public int CarTypeId { get; set; }

        public string CarTypeTitle { get; set; }

        public int CarTipId { get; set; }

        public string CarTipTitle { get; set; }

        public bool Visible { get; set; }

        public List<string> CarTipImageUrls { get; set; }
    }
}
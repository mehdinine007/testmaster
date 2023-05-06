using System;
using System.Collections.Generic;

namespace OrderManagement.Application.Contracts
{
    public class SalePlanDto
    {
        public int Id { get; set; }

        public DateTime SaleStartDate { get; set; }

        public DateTime SaleEndDate { get; set; }

        public int EsaleTypeId { get; set; }

        public string EsaleTypeName { get; set; }

        public int CarTipId { get; set; }

        public string CarTipTitle { get; set; }

        public decimal MinimumAmountOfProxyDeposit { get; set; }

        public List<string> CarTipImageUrls { get; set; }
    }
}
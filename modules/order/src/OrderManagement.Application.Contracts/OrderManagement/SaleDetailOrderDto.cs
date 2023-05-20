using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Application.Contracts
{
    public class SaleDetailOrderDto
    {
        public int Id { get; set; }
        public Guid UID { get; set; }
        public int SaleId { get; set; }
        public int ESaleTypeId { get; set; }
        public DateTime SalePlanStartDate { get; set; }

        public DateTime SalePlanEndDate { get; set; }

        public int EsaleTypeId { get; set; }
        [Column(TypeName = "decimal(15)")]
        public decimal MinimumAmountOfProxyDeposit { get; set; }

    }
}
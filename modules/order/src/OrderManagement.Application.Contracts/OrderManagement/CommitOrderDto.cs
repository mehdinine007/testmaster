using System;

namespace OrderManagement.Application.Contracts
{
    public class CommitOrderDto
    {
        public Guid SaleDetailUId { get; set; }
        public int PriorityId { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string ChassiNo { get; set; }
        public string Vehicle { get; set; }

    }
}
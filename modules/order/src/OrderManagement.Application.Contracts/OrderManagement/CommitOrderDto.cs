using OrderManagement.Domain.Shared;
using System;

namespace OrderManagement.Application.Contracts
{
    public class CommitOrderDto
    {
        public Guid SaleDetailUId { get; set; }
       // public int PriorityId { get; set; }
        public PriorityEnum? PriorityId { get; set; }
    }
}
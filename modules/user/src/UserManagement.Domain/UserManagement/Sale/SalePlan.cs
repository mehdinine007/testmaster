using Abp.Domain.Entities.Auditing;
using System;
using UserManagement.Domain.UserManagement.Bases;

namespace UserManagement.Domain.UserManagement.Sale
{
    public class SalePlan : FullAuditedAggregateRoot<int>
    {
        public DateTime SaleStartDate { get; set; }

        public DateTime SaleEndDate { get; set; }

        public int EsaleTypeId { get; set; }

        public int CarTipId { get; set; }

        public decimal MinimumAmountOfProxyDeposit { get; set; }

        public virtual CarTip CarTip { get; set; }

        public virtual ESaleType ESaleType { get; set; }
    }
}

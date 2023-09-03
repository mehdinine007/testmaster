using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using UserManagement.Domain.UserManagement.Bases;

namespace UserManagement.Domain.UserManagement.Sale
{
    public class ESaleType : FullAuditedAggregateRoot<int>
    {
        private ICollection<SalePlan> _salePlans;

        private ICollection<SaleDetail> _saleDetail;

        public string SaleTypeName { get; set; }

        public virtual ICollection<SalePlan> SalePlans
        {
            get => _salePlans ?? (_salePlans = new List<SalePlan>());
            protected set => _salePlans = value;
        }

        public virtual ICollection<SaleDetail> SaleDetails
        {
            get => _saleDetail ??(_saleDetail = new List<SaleDetail>());
            protected set => _saleDetail = value;
        }


        public virtual ICollection<Season_Company_CarTip> SeasonCompanyCarTip { get; set; }

    }
}

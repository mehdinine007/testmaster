using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class ESaleType : FullAuditedEntity<int>
    {
        //private ICollection<SalePlan> _salePlans;

        private ICollection<SaleDetail> _saleDetail;

        public string SaleTypeName { get; set; }

        //public virtual ICollection<SalePlan> SalePlans
        //{
        //    get => _salePlans ?? (_salePlans = new List<SalePlan>());
        //    protected set => _salePlans = value;
        //}

        public virtual ICollection<SaleDetail> SaleDetails
        {
            get => _saleDetail ?? (_saleDetail = new List<SaleDetail>());
            protected set => _saleDetail = value;
        }


        public virtual ICollection<Season_Company_CarTip> SeasonCompanyCarTip { get; set; }

    }

}

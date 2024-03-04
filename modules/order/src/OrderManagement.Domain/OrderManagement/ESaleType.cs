using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class ESaleType : FullAuditedEntity<int>
    {
        //private ICollection<SalePlan> _salePlans;

        private ICollection<SaleDetail> _saleDetail;

        private ICollection<SeasonCompanyProduct> _seasonCompanyProducts;

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


        public virtual ICollection<Season_Product_Category> SeasonCompanyCarTip { get; set; }

        public ICollection<SeasonCompanyProduct> SeasonCompanyProducts
        {
            get => _seasonCompanyProducts ?? (_seasonCompanyProducts = new List<SeasonCompanyProduct>());
            protected set => _seasonCompanyProducts = value;
        }
    }

}

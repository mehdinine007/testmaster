using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class CarTip : FullAuditedEntity<int>
    {
        private ICollection<SaleDetail> _saleDetails;

        //private ICollection<SalePlan> _salePlans;

        private ICollection<CarTip_Gallery_Mapping> _carTip_Gallery_Mappings;

        public string Title { get; set; }

        public int CarTypeId { get; set; }

        public virtual CarType CarType { get; set; }

        public virtual ICollection<CarTip_Gallery_Mapping> CarTip_Gallery_Mappings
        {
            get => _carTip_Gallery_Mappings ?? (_carTip_Gallery_Mappings = new List<CarTip_Gallery_Mapping>());
            protected set => _carTip_Gallery_Mappings = value;
        }

        public virtual ICollection<SaleDetail> SaleDetails
        {
            get => _saleDetails ?? (_saleDetails = new List<SaleDetail>());
            protected set => _saleDetails = value;
        }

        //public virtual ICollection<SalePlan> SalePlans
        //{
        //    get => _salePlans ?? (_salePlans = new List<SalePlan>());
        //    protected set => _salePlans = value;
        //}


        public virtual ICollection<Season_Company_CarTip> SeasonCompanyCarTip { get; set; }

    }

}

using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Gallery : FullAuditedEntity<int>
    {
        private ICollection<CarTip_Gallery_Mapping> _carTip_Gallery_Mappings;
        public string ImageUrl { get; set; }

        public virtual Company CompanyLogo { get; set; }
        public virtual Company CompanyLogoInPage { get; set; }
        public virtual Company CompanyBanner { get; set; }

        public virtual Bank Bank { get; set; }

        public virtual ICollection<CarTip_Gallery_Mapping> CarTip_Gallery_Mappings
        {
            get => _carTip_Gallery_Mappings ?? (_carTip_Gallery_Mappings = new List<CarTip_Gallery_Mapping>());
            protected set => _carTip_Gallery_Mappings = value;
        }
    }

}

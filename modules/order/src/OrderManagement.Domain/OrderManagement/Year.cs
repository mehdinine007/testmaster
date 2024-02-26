using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Year : FullAuditedEntity<int>
    {
        private ICollection<SeasonCompanyProduct> _seasonCompanyProducts;

        public string Title { get; set; }

        public ICollection<SeasonCompanyProduct> SeasonCompanyProducts
        {
            get => _seasonCompanyProducts ?? (_seasonCompanyProducts = new List<SeasonCompanyProduct>());
            protected set => _seasonCompanyProducts = value;
        }
    }

}

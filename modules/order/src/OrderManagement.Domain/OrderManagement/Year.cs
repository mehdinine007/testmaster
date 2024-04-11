using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Year : FullAuditedEntity<int>
    {
        private ICollection<SaleDetailAllocation> _seasonCompanyProducts;

        public string Title { get; set; }

        public ICollection<SaleDetailAllocation> SeasonCompanyProducts
        {
            get => _seasonCompanyProducts ?? (_seasonCompanyProducts = new List<SaleDetailAllocation>());
            protected set => _seasonCompanyProducts = value;
        }
    }

}

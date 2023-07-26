using OrderManagement.Domain.OrderManagement;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class SaleSchema : FullAuditedEntity<int>
    {
        private ICollection<SaleDetail> _saleDetails;

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int SaleStatus { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails
        { 
            get => _saleDetails ?? (_saleDetails = new List<SaleDetail>());
            protected set => _saleDetails = value;
        }

    }
}

using OrderManagement.Domain.Shared;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class ProductAndCategory : FullAuditedEntity<int>
    {
        private ICollection<ProductAndCategory> _childrens;

        public string Code { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public ProductAndCategoryType Type { get; set; }

        public int LevelId { get; set; }

        public virtual ProductAndCategory Parent { get; protected set; }

        public virtual ICollection<ProductAndCategory> Childrens
        {
            get => _childrens ?? (_childrens = new List<ProductAndCategory>());
            protected set => _childrens = value;
        }
    }
}

using OrderManagement.Domain.Shared;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class ProductAndCategory : FullAuditedEntity<int>
    {
        private ICollection<ProductAndCategory> _childrens;

        private ICollection<Season_Product_Category> _categorySeason;

        private ICollection<Season_Product_Category> _productSeason;

        public string Code { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public ProductAndCategoryType Type { get; set; }

        public int LevelId { get; set; }

        public bool Active { get; set; }

        public virtual ProductAndCategory Parent { get; protected set; }

        public virtual ICollection<Season_Product_Category> CategorySeason
        {
            get => _categorySeason ?? (_categorySeason = new List<Season_Product_Category>());
            protected set => _categorySeason = value;
        }

        public virtual ICollection<Season_Product_Category> ProductSeason
        {
            get => _productSeason ?? (_productSeason = new List<Season_Product_Category>());
            protected set => _productSeason = value;
        }

        public virtual ICollection<ProductAndCategory> Childrens
        {
            get => _childrens ?? (_childrens = new List<ProductAndCategory>());
            protected set => _childrens = value;
        }


        public ProductAndCategory()
        {
            Childrens = new HashSet<ProductAndCategory>();
        }
    }
}

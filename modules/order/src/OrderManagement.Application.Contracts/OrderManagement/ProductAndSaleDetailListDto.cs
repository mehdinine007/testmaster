using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class ProductAndSaleDetailListDto
    {

        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public ProductAndCategoryType Type { get; set; }

        public int LevelId { get; set; }

        public bool HasChild { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<ProductAndCategoryWithChildDto> Childrens { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
        public List<SaleDetailListDto> SaleDetails { get; set; }
        public  ProductLevelDto ProductLevel { get; protected set; }



    }
}

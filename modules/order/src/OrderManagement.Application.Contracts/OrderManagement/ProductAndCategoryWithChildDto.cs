﻿using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class ProductAndCategoryWithChildDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public ProductAndCategoryType Type { get; set; }

        public int LevelId { get; set; }

        public bool HasChild { get; set; }

        public bool Active { get; set; }
        public int Priority { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
        public List<PropertyCategoryDto> PropertyCategories { get; set; }

        public virtual ICollection<ProductAndCategoryWithChildDto> Childrens { get; set; }
        public List<SaleDetailListDto> SaleDetails { get; set; }
        public ProductLevelDto ProductLevel { get; protected set; }
        public int OrganizationId { get; set; }
    }
}
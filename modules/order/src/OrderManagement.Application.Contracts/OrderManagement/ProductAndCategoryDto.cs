namespace OrderManagement.Application.Contracts
{
    public class ProductAndCategoryDto
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public string ProductAndCategoryTypeTitle { get; set; }

        public int ProductAndCategoryTypeCode { get; set; }

        public int LevelId { get; set; }
    }
}
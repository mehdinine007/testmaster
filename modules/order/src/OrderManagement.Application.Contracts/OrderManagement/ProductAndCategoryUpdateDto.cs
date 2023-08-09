namespace OrderManagement.Application.Contracts
{
    public class ProductAndCategoryUpdateDto
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public string Title { get; set; }
    }
}
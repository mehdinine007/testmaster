using System.ComponentModel.DataAnnotations;

namespace OrderManagement
{
    public class UpdateOrderDto
    {
        [Required]
        [StringLength(OrderConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(OrderConsts.MaxImageNameLength)]
        public string ImageName { get; set; }

        public float Price { get; set; }

        public int StockCount { get; set; }
    }
}
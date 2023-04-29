using System.ComponentModel.DataAnnotations;

namespace OrderManagement
{
    public class CreateOrderDto
    {
        [Required]
        [StringLength(OrderConsts.MaxCodeLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(OrderConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(OrderConsts.MaxImageNameLength)]
        public string ImageName { get; set; }

        public float Price { get; set; }

        public int StockCount { get; set; }
    }
}
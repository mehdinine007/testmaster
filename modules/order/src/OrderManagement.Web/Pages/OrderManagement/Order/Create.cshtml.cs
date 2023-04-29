using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace OrderManagement.Pages.OrderManagement.Orders
{
    public class CreateModel : AbpPageModel
    {
        private readonly IOrderAppService _OrderAppService;

        [BindProperty]
        public OrderCreateViewModel Order { get; set; } = new OrderCreateViewModel();

        public CreateModel(IOrderAppService OrderAppService)
        {
            _OrderAppService = OrderAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var createOrderDto = ObjectMapper.Map<OrderCreateViewModel, CreateOrderDto>(Order);

            await _OrderAppService.CreateAsync(createOrderDto);

            return NoContent();
        }

        public class OrderCreateViewModel
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
}
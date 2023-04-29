using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace OrderManagement.Pages.OrderManagement.Orders
{
    public class EditModel : AbpPageModel
    {
        private readonly IOrderAppService _OrderAppService;

        [BindProperty]
        public OrderEditViewModel Order { get; set; } = new OrderEditViewModel();

        public EditModel(IOrderAppService OrderAppService)
        {
            _OrderAppService = OrderAppService;
        }

        public async Task<ActionResult> OnGetAsync(Guid OrderId)
        {
            var OrderDto = await _OrderAppService.GetAsync(OrderId);

            Order = ObjectMapper.Map<OrderDto, OrderEditViewModel>(OrderDto);

            return Page();
        }

        public async Task OnPostAsync()
        {
            await _OrderAppService.UpdateAsync(Order.Id, new UpdateOrderDto()
            {
                Name = Order.Name,
                Price = Order.Price,
                StockCount = Order.StockCount
            });
        }

        public class OrderEditViewModel
        {
            [HiddenInput]
            [Required]
            public Guid Id { get; set; }

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
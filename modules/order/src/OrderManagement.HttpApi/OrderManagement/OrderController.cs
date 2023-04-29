using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace OrderManagement
{
    [RemoteService]
    [Area("OrderManagement")]
    [Route("api/OrderManagement/Orders")]
    public class OrderController : AbpController, IOrderAppService
    {
        private readonly IOrderAppService _OrderAppService;

        public OrderController(IOrderAppService OrderAppService)
        {
            _OrderAppService = OrderAppService;
        }

        [HttpGet]
        [Route("")]
        public Task<PagedResultDto<OrderDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input)
        {
            return _OrderAppService.GetListPagedAsync(input);
        }

        [HttpGet]
        [Route("all")]
        public Task<ListResultDto<OrderDto>> GetListAsync()
        {
            return _OrderAppService.GetListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<OrderDto> GetAsync(Guid id)
        {
            return _OrderAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            return _OrderAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input)
        {
            return _OrderAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public Task DeleteAsync(Guid id)
        {
            return _OrderAppService.DeleteAsync(id);
        }
    }
}

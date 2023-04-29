using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement
{
    [Authorize(OrderManagementPermissions.Orders.Default)]
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly OrderManager _OrderManager;
        private readonly IRepository<Order, Guid> _OrderRepository;

        public OrderAppService(OrderManager OrderManager, IRepository<Order, Guid> OrderRepository)
        {
            _OrderManager = OrderManager;
            _OrderRepository = OrderRepository;
        }

        public async Task<PagedResultDto<OrderDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input)
        {
            await NormalizeMaxResultCountAsync(input);

            var Orders = await (await _OrderRepository.GetQueryableAsync())
                .OrderBy(input.Sorting ?? "Name")
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            var totalCount = await _OrderRepository.GetCountAsync();

            var dtos = ObjectMapper.Map<List<Order>, List<OrderDto>>(Orders);

            return new PagedResultDto<OrderDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<OrderDto>> GetListAsync() //TODO: Why there are two GetList. GetListPagedAsync would be enough (rename it to GetList)!
        {
            var Orders = await _OrderRepository.GetListAsync();

            var OrderList =  ObjectMapper.Map<List<Order>, List<OrderDto>>(Orders);

            return new ListResultDto<OrderDto>(OrderList);
        }

        public async Task<OrderDto> GetAsync(Guid id)
        {
            var Order = await _OrderRepository.GetAsync(id);

            return ObjectMapper.Map<Order, OrderDto>(Order);
        }

        [Authorize(OrderManagementPermissions.Orders.Create)]
        public async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            var Order = await _OrderManager.CreateAsync(
                input.Code,
                input.Name,
                input.Price,
                input.StockCount,
                input.ImageName
            );

            return ObjectMapper.Map<Order, OrderDto>(Order);
        }

        [Authorize(OrderManagementPermissions.Orders.Update)]
        public async Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input)
        {
            var Order = await _OrderRepository.GetAsync(id);

            Order.SetName(input.Name);
            Order.SetPrice(input.Price);
            Order.SetStockCount(input.StockCount);
            Order.SetImageName(input.ImageName);

            return ObjectMapper.Map<Order, OrderDto>(Order);
        }

        [Authorize(OrderManagementPermissions.Orders.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _OrderRepository.DeleteAsync(id);
        }

        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(OrderManagementSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}

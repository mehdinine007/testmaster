using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement
{
    public interface IOrderAppService : IApplicationService
    {
        Task<PagedResultDto<OrderDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input);

        Task<ListResultDto<OrderDto>> GetListAsync();

        Task<OrderDto> GetAsync(Guid id);

        Task<OrderDto> CreateAsync(CreateOrderDto input);

        Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input);

        Task DeleteAsync(Guid id);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement
{
    public class PublicOrderAppService : ApplicationService, IPublicOrderAppService
    {
        private readonly IRepository<Order, Guid> _OrderRepository;

        public PublicOrderAppService(IRepository<Order, Guid> OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public async Task<ListResultDto<OrderDto>> GetListAsync()
        {
            return new ListResultDto<OrderDto>(
                ObjectMapper.Map<List<Order>, List<OrderDto>>(
                    await _OrderRepository.GetListAsync()
                )
            );
        }
    }
}

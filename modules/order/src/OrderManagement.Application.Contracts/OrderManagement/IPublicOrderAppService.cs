using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement
{
    public interface IPublicOrderAppService : IApplicationService
    {
        Task<ListResultDto<OrderDto>> GetListAsync();
    }
}
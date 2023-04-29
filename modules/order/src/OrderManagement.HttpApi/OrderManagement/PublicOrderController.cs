using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace OrderManagement
{
    [RemoteService]
    [Area("OrderManagement")]
    [Route("api/OrderManagement/public/Orders")]
    public class PublicOrderController : AbpController, IPublicOrderAppService
    {
        private readonly IPublicOrderAppService _publicOrderAppService;

        public PublicOrderController(IPublicOrderAppService publicOrderAppService)
        {
            _publicOrderAppService = publicOrderAppService;
        }

        [HttpGet]
        public Task<ListResultDto<OrderDto>> GetListAsync()
        {
            return _publicOrderAppService.GetListAsync();
        }
    }
}

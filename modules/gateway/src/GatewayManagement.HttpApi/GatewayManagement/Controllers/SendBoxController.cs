#region NS
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using Volo.Abp.Auditing;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
#endregion

namespace SendBoxController
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/SendBoxService/[action]")]
    public class SendBoxController : Controller
    {
        private readonly ISendBoxService _sendBoxService;
        public SendBoxController(ISendBoxService sendBoxService)
        {
            _sendBoxService = sendBoxService;
        }

        [HttpPost]
        public async Task<SendBoxServiceDto> SendSms(SendBoxServiceInput input)
        {
            return await _sendBoxService.SendService(input);
        }

    }
}

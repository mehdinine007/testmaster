using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using Volo.Abp.Auditing;

namespace SendBoxController
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/SendBoxService/[action]")]
    public class SendBoxController : Controller
    {
        private readonly ISendSmsService _sendSmsService;
        public SendBoxController(ISendSmsService sendSmsService)
        {
            _sendSmsService = sendSmsService;
        }

        [HttpPost]
        public async Task<SendBoxServiceDto> SendSms(SendBoxServiceInput input) 
        {
           return await _sendSmsService.MagfaSendSms(input.Recipient, input.Text);
        } 

    }
}

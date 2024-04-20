#region NS
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using Volo.Abp.Auditing;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale;
using GatewayManagement.Application.Contracts.Dtos.Esale;
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


        [HttpPost]
        public async Task<CreateSignOutputDto> Create(CreateSignDto input)
        {
            return await _sendBoxService.CreateSign(input);
        }
        [HttpGet]
        public async Task<ResponseInquiryIranSign> Inquiry(Guid workflowTicket)
        {
            return await _sendBoxService.InquirySign(workflowTicket);
        }
    }
}

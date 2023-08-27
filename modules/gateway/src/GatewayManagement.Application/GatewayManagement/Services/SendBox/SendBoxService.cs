using Esale.SendBox.Providers.Magfa;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.Enums;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices.Esale;
using GatewayManagement.Application.GatewayManagement.Services.SendBox;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GatewayManagement.Application.GatewayManagement.Services.SendBox
{
    public class SendBoxService : IApplicationService, ISendBoxService
    {
        private IConfiguration _configuration; 

        public SendBoxService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SendBoxServiceDto> SendService(SendBoxServiceInput sendBoxService)
        {
            SendBoxServiceDto _ret = null;
            if (sendBoxService.Provider == ProviderSmsTypeEnum.Magfa && sendBoxService.Type == TypeMessageEnum.Sms)
            {
                var _magfa = new MagfaSendSms(JsonConvert.DeserializeObject<MagfaConfig>(_configuration.GetSection("SendBoxConfig:Sms:Magfa").Value));
                _ret = await _magfa.Send(sendBoxService.Text, sendBoxService.Recipient);
                return _ret;
            }
            return new SendBoxServiceDto
            {
                Success = false,
                DataResult = "None Service Send",
                MessageCode = 97
            };
        }
    }
}

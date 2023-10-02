#region NS
using Esale.SendBox.Providers.Magfa;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.Enums;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Application.Services;
#endregion

namespace GatewayManagement.Application.GatewayManagement.Services.SendBox
{
    public class SendBoxService : ApplicationService, ISendBoxService
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
                var _magfa = new MagfaSendSms(new MagfaConfig()
                {
                    BaseUrl = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:BaseUrl"),
                    Domain = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:Domain"),
                    SenderNumber = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:SenderNumber"),
                    UserName = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:UserName"),
                    Password = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:Password")
                });
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

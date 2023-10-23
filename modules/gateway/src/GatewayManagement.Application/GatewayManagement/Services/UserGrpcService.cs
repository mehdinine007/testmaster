#region NS
using Esale.GetwayServiceGrpc;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using Grpc.Core;
#endregion

namespace GatewayManagement.Application.GatewayManagement.Services
{
    public class UserGrpcService : GetwayServiceGrpc.GetwayServiceGrpcBase
    {
        private readonly ICaptchaService _captchaService;
        private readonly ISendBoxService _sendBoxervice;

        public UserGrpcService(ICaptchaService captchaService, ISendBoxService sendBox)
        {
            _captchaService = captchaService;
            _sendBoxervice = sendBox;
        }
        public override async Task<Esale.GetwayServiceGrpc.HttpResponseMessageDto> ReCaptcha(Esale.GetwayServiceGrpc.ContentInputDto request, ServerCallContext context)
        {
            var getCaptcha = await _captchaService.ReCaptcha(new Contracts.GatewayManagement.Dtos.ContentInputDto
            {
                ContentValue = request.ContentValue,
                KeyValue = request.KeyValue
            });
            return await Task.FromResult(new Esale.GetwayServiceGrpc.HttpResponseMessageDto
            {
                Success = getCaptcha.Success,
                Error = getCaptcha.Error,
                ErrorCode = getCaptcha.ErrorCode
            });

        }
        public override async Task<Esale.GetwayServiceGrpc.SendBoxServiceDto> SendService(Esale.GetwayServiceGrpc.SendBoxServiceInput input, ServerCallContext context)
        {
            var sendSms = await _sendBoxervice.SendService(new Contracts.GatewayManagement.Dtos.SendBoxServiceInput
            {
                Recipient = input.Recipient,
                Text = input.Text,
                Type = (Contracts.GatewayManagement.Enums.TypeMessageEnum)input.Type,
                Provider = (Contracts.GatewayManagement.Enums.ProviderSmsTypeEnum)input.Provider
            });

            var sendBoxServiceDto = new Esale.GetwayServiceGrpc.SendBoxServiceDto
            {
                Success = sendSms.Success,
                MessageCode = sendSms.MessageCode,
                Message = sendSms.Message ?? string.Empty
            };

            return sendBoxServiceDto;
        }
    }
}
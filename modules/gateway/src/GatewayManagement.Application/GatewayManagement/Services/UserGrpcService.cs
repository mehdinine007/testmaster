using Esale.GetwayServiceGrpc;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices.Esale;
using Grpc.Core;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;


namespace GatewayManagement.Application.GatewayManagement.Services
{
    public class UserGrpcService : GetwayServiceGrpc.GetwayServiceGrpcBase
    {
        private readonly ICaptchaService _esalervice;
        private readonly ISendBoxService _sendBoxervice;

        public UserGrpcService(ICaptchaService esalervice, ISendBoxService sendBox)
        {
            _esalervice = esalervice;
            _sendBoxervice = sendBox;
        }
        public override async Task<Esale.GetwayServiceGrpc.HttpResponseMessageDto> GetCaptcha(Esale.GetwayServiceGrpc.ContentInputDto request, ServerCallContext context)
        {
            var getCaptcha = await _esalervice.GetCaptcha(new Contracts.GatewayManagement.Dtos.ContentInputDto
            {
                ContentValue = request.ContentValue,
                KeyValue = request.KeyValue
            });
            return await Task.FromResult(new Esale.GetwayServiceGrpc.HttpResponseMessageDto
            {
                IsSuccessStatusCode = getCaptcha.IsSuccessStatusCode,
                StringContent = getCaptcha.StringContent
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
using GatewayManagement.Application.Contracts.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using GatewayManagement.Application.SignGrpc;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.GatewayManagement.Services
{
    public class SignGrpcServiceProvider : SignGrpc.SignGrpcService.SignGrpcServiceBase
    {
        private readonly ISendBoxService _sendBoxervice;

        public SignGrpcServiceProvider(ISendBoxService sendBox)
        {
            _sendBoxervice = sendBox;
        }
        public override async Task<CreateSignGrpcOutput> CreateSign(CreateSignGrpcInput request, ServerCallContext context)
        {
            var result = await _sendBoxervice.CreateSign(JsonConvert.DeserializeObject<CreateSignDto>(JsonConvert.SerializeObject(request)));
            var _ret = JsonConvert.DeserializeObject<CreateSignGrpcOutput>(JsonConvert.SerializeObject(result));
            return _ret;
        }
    }

}

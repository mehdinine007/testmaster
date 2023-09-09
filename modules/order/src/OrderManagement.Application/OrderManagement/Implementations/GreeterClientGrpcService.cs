#region NS
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using GrpcClient;
using Microsoft.AspNetCore.Mvc;
#endregion


namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class GreeterClientGrpcService : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GreeterClientGrpcService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Method
        public async Task<IActionResult> GetId()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new Greeter.GreeterClient(channel);
            var xx = client.SayHello(new HelloRequest { Name = "kokokoko" });
            return Ok(xx.Message);
        }
        #endregion

    }
}
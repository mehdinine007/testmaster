using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient;
using System;
using System.Collections.Generic;
using System.Linq;
#region NS
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
#endregion


namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class GreeterService : Greeter.GreeterBase, ITransientDependency
    {
        #region Method
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            string message = $"Hello, {request.Name}!";
            return Task.FromResult(new HelloReply { Message = message });
        }
        #endregion

    }
}

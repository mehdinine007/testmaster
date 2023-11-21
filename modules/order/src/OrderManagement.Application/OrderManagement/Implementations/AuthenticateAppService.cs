using OrderManagement.Application.Contracts.OrderManagement.Services;
using System.Threading.Tasks;
using IFG.Core.Infrastructures.TokenAuth;
using Volo.Abp.Application.Services;
using OrderManagement.Application.Contracts.Services;
using Volo.Abp;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class AuthenticateAppService : ApplicationService, IAuthenticateAppService
{
    private readonly IEsaleGrpcClient _esaleGrpcClient;

    public AuthenticateAppService(IEsaleGrpcClient esaleGrpcClient)
    {
        _esaleGrpcClient = esaleGrpcClient;
    }

    public async Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto input)
    {
        var auth = await _esaleGrpcClient.Athenticate(input);
        if (auth.Success)
        {
            return auth.Data;
        }
        throw new UserFriendlyException(auth.Message, auth.ErrorCode.ToString());

    }
}
#region NS
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using Volo.Abp.Application.Services;
#endregion

namespace GatewayManagement.Application.Contracts.GatewayManagement.IServices
{
    public interface ISendBoxService : IApplicationService
    {
        Task<SendBoxServiceDto> SendService(SendBoxServiceInput sendBoxService);
    }
}

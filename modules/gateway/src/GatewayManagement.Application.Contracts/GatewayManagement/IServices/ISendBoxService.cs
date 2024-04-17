#region NS
using GatewayManagement.Application.Contracts.Dtos.Esale.IranSign;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign;
using Volo.Abp.Application.Services;
#endregion

namespace GatewayManagement.Application.Contracts.GatewayManagement.IServices
{
    public interface ISendBoxService : IApplicationService
    {
        Task<SendBoxServiceDto> SendService(SendBoxServiceInput sendBoxService);
        Task<CreateIranSignOutput> CreateSign(CreateIranSignDto createIranSignDto);
        Task<ResponseInquiryIranSign> InquirySign(Guid workflowTicket);
    }
}

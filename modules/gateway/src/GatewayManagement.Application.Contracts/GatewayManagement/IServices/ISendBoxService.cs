#region NS
using GatewayManagement.Application.Contracts.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign;
using Volo.Abp.Application.Services;
#endregion

namespace GatewayManagement.Application.Contracts.GatewayManagement.IServices
{
    public interface ISendBoxService : IApplicationService
    {
        Task<SendBoxServiceDto> SendService(SendBoxServiceInput sendBoxService);
        Task<CreateSignOutputDto> CreateSign(CreateSignDto createSignDto);
        Task<ResponseInquiryIranSign> InquirySign(Guid workflowTicket);
    }
}

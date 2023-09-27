using UserManagement.Application.Contracts.Models.SendBox;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IGetwayGrpcClient : IApplicationService
    {
        Task<HttpResponseMessageDto> GetCaptcha(ContentInputDto Content);
        Task<SendBoxServiceDto> SendService(SendBoxServiceInput input);
    }
}

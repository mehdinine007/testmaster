#region NS
using UserManagement.Application.Contracts.Models.SendBox;
using Volo.Abp.Application.Services;
#endregion

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IGetwayGrpcClient : IApplicationService
    {
        Task<HttpResponseMessageDto> ReCaptcha(ContentInputDto Content);
        Task<SendBoxServiceDto> SendService(SendBoxServiceInput input);
    }
}

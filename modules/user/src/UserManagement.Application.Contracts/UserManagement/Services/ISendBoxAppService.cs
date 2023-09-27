using Volo.Abp.Application.Services;
using Esale.Core.Utility.Results;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface ISendBoxAppService : IApplicationService
    {
        Task<IResult> SendSms(SendSMSDto input);
    }
}

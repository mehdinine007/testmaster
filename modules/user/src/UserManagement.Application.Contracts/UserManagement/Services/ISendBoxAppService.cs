#region NS
using Volo.Abp.Application.Services;
using IFG.Core.Utility.Results;
using UserManagement.Application.Contracts.Models;
#endregion

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface ISendBoxAppService : IApplicationService
    {
        Task<IResult> SendSms(SendSMSDto input);
    }
}

using Esale.Core.Utility.Results;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.UserManagement.SendBox.Dtos;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace UserManagement.HttpApi.UserManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/[controller]/[action]")]
public class SendBoxAppController : AbpController
{
    private readonly ISendBoxAppService _sendBoxAppService;
    public SendBoxAppController(ISendBoxAppService sendBoxAppService)
    {
        _sendBoxAppService = sendBoxAppService;
    }

    [HttpPost]
    public async Task<IResult> SendSms(SendSMSDto input)
    {
        IResult result = await _sendBoxAppService.SendSms(input);
        return result;
    }
}
using Esale.Core.Utility.Results;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Models.SendBox;
using UserManagement.Application.Contracts.UserManagement.Services;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace UserManagement.HttpApi.UserManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/[controller]/[action]")]
public class SendBoxController : Controller
{
    private readonly ISendBoxAppService _sendBoxAppService;
   

    public SendBoxController(ISendBoxAppService sendBoxAppService)
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
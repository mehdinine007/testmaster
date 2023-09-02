using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.OrderManagement;
using Volo.Abp.Application.Dtos;
using Esale.Share.Authorize;
using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;
using OrderManagement.Application.Contracts;
using Esale.Core.Utility.Tools;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/SiteStructureService/[action]")]
//[UserAuthorization]
public class SiteStructureController : Controller
{
    private readonly ISiteStructureService _siteStructureService;
    public SiteStructureController(ISiteStructureService siteStructureService)
        => _siteStructureService = siteStructureService;

    [HttpGet]
    public Task<SiteStructureDto> GetById(int id, string attachmentType)
    => _siteStructureService.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType));


    [HttpGet]
    public Task<List<SiteStructureDto>> GetList(SiteStructureQueryDto siteStructureQuery)
    => _siteStructureService.GetList(siteStructureQuery);
    

    [HttpPost]
    public Task<SiteStructureDto> Add(SiteStructureAddOrUpdateDto siteStructureDto)
    => _siteStructureService.Add(siteStructureDto);

    [HttpPut]
    public Task<SiteStructureDto> Update(SiteStructureAddOrUpdateDto siteStructureDto)
    => _siteStructureService.Update(siteStructureDto);

    [HttpDelete]
    public Task<bool> Delete(int id)
    => _siteStructureService.Delete(id);

    [HttpPost]
    public Task<bool> UploadFile([FromForm] UploadFileDto uploadFile)
   => _siteStructureService.UploadFile(uploadFile);
}

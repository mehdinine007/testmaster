using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using Volo.Abp.Application.Dtos;
using OrderManagement.Application.Contracts.OrderManagement;
using IFG.Core.Utility.Tools;
using OrderManagement.Domain.Shared;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/OrganizationService/[action]")]
public class OrganizationController : Controller
{
    private readonly IOrganizationService _organizationService;
    public OrganizationController(IOrganizationService organizationService)
        => _organizationService = organizationService;

    [HttpDelete]
    public Task<bool> Delete(int id)
    => _organizationService.Delete(id);
    [HttpGet]
    public Task<List<OrganizationDto>> GetAll(string attachmentType, string attachmentlocation)
    => _organizationService.GetAll(EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));

    [HttpGet]
    public Task<OrganizationDto> GetById(int id, string attachmentType, string attachmentlocation)
    => _organizationService.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));
  

    [HttpPost]
    public Task<int> Save(OrganizationInsertDto orgDto)
    => _organizationService.Save(orgDto);
    [HttpPut]
    public Task<int> Update(OrganizationUpdateDto orgDto)
    => _organizationService.Update(orgDto);

    [HttpPost]
    public Task<bool> UploadFile(UploadFileDto uploadFile)
         => _organizationService.UploadFile(uploadFile);
    [HttpPost]
    public Task<bool> Move(OrganizationPriorityDto input)
      => _organizationService.Move(input);
}

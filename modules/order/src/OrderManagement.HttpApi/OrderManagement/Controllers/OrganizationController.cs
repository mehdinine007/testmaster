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
using System;

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
    public async Task<bool> Delete(int id)
    =>await _organizationService.Delete(id);
    [HttpGet]
    public async Task<List<OrganizationDto>> GetList(bool? isActive,string attachmentType, string attachmentlocation)
    => await  _organizationService.GetList(new OrganizationQueryDto { IsActive = isActive ,AttachmentEntityType= EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType),AttachmentLocation= EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation) });
    [HttpGet]
    public async Task<OrganizationDto> GetById(int id, string attachmentType, string attachmentlocation)
    => await _organizationService.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));
  

    [HttpPost]
    public async Task<OrganizationDto> Add(OrganizationAddOrUpdateDto orgDto)
    =>await _organizationService.Add(orgDto);
    [HttpPut]
    public async Task<OrganizationDto> Update(OrganizationAddOrUpdateDto orgDto)
    => await _organizationService.Update(orgDto);

    [HttpPost]
    public async Task<Guid> UploadFile([FromForm] UploadFileDto uploadFile)
         =>await _organizationService.UploadFile(uploadFile);
    [HttpPost]
    public async Task<bool> Move(OrganizationPriorityDto input)
      =>await _organizationService.Move(input);
}

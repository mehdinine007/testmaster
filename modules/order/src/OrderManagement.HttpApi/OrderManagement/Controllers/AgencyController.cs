using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.OrderManagement.Implementations;
using Volo.Abp.Application.Dtos;
using Esale.Share.Authorize;
using OrderManagement.Domain.Shared;
using OrderManagement.Application.Contracts.OrderManagement;
using IFG.Core.Utility.Tools;
using OrderManagement.Domain.Shared.OrderManagement.Enums;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/AgencyService/[action]")]
public class AgencyController : Controller
{

    private readonly IAgencyService _agencyServicecs;
    public AgencyController(IAgencyService agencyServicecs)
        => _agencyServicecs = agencyServicecs;



    [HttpDelete]
    public async Task<bool> Delete(int id)
     => await _agencyServicecs.Delete(id);
    [HttpGet]
    public async Task<PagedResultDto<AgencyDto>> GetAgencies(int pageNo, int sizeNo)
     => await _agencyServicecs.GetAgencies(pageNo, sizeNo);
    [HttpPost]
    public async Task<AgencyDto> Add(AgencyCreateDto agencyDto)
     => await _agencyServicecs.Add(agencyDto);

    [HttpPut]
    public async Task<AgencyDto> Update(AgencyCreateOrUpdateDto agencyDto)
     => await _agencyServicecs.Update(agencyDto);
    [HttpGet]
    public async Task<AgencyDto> GetById(int id, string attachmentType, string attachmentlocation)
    => await _agencyServicecs.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));
    [HttpGet]
    public async Task<List<AgencyDto>> GetList(int? provinceId, int? cityId, string code, string name, AgencyTypeEnum? agencyType, string attachmentType, string attachmentlocation)
    =>await _agencyServicecs.GetList(new AgencyQueryDto { ProvinceId= provinceId ,CityId=cityId,Code=code, Name=name,AgencyType=agencyType,
        AttachmentEntityType= EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType)
        ,AttachmentLocation= EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation)
    });

    [HttpPost]
    public async Task<Guid> UploadFile([FromForm] UploadFileDto uploadFile)
        => await _agencyServicecs.UploadFile(uploadFile);
}

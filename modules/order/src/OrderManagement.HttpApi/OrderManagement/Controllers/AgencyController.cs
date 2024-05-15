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

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/AgencyService/[action]")]
public class AgencyController : Controller, IAgencyService
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
    public async Task<int> Add(AgencyDto agencyDto)
     => await _agencyServicecs.Add(agencyDto);

    [HttpPut]
    public async Task<int> Update(AgencyDto agencyDto)
     => await _agencyServicecs.Update(agencyDto);

    public async Task<AgencyDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    =>await _agencyServicecs.GetById(id, attachmentType, attachmentlocation); 

    public async Task<List<AgencyDto>> GetList(AgencyQueryDto agencyQueryDto)
    =>await _agencyServicecs.GetList(agencyQueryDto);
}

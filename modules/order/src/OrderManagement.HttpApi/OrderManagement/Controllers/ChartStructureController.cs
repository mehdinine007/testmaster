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

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/ChartStructureService/[action]")]
//[UserAuthorization]
public class ChartStructureController : Controller
{
    private readonly IChartStructureService _chartStructureService;
    public ChartStructureController(IChartStructureService chartStructureService)
        => _chartStructureService = chartStructureService;


    [HttpGet]
    public Task<List<ChartStructureDto>> GetList(AttachmentEntityTypeEnum? attachmentType)
    => _chartStructureService.GetList(attachmentType);

    [HttpPost]
    public Task<bool> UploadFile([FromForm]UploadFileDto uploadFile)
    => _chartStructureService.UploadFile(uploadFile);
    [HttpGet]
    public Task<ChartStructureDto> GetById(int id)
    => _chartStructureService.GetById(id);
}

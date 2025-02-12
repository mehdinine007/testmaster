﻿using System;
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
using IFG.Core.Utility.Tools;

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
    public Task<List<ChartStructureDto>> GetList(string attachmentType, string attachmentlocation)
    => _chartStructureService.GetList(EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));

    [HttpPost]
    public Task<bool> UploadFile([FromForm]UploadFileDto uploadFile)
    => _chartStructureService.UploadFile(uploadFile);
    [HttpGet]
    public Task<ChartStructureDto> GetById(int id, string attachmentType, string attachmentlocation)
    => _chartStructureService.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));

    [HttpPost]
    public Task<ChartStructureDto> Add(ChartStructureCreateOrUpdateDto chartStructureCreateOrUpdateDto)
    => _chartStructureService.Add(chartStructureCreateOrUpdateDto);
    [HttpPost]
    public Task<ChartStructureDto> Update(ChartStructureCreateOrUpdateDto chartStructureCreateOrUpdateDto)
         => _chartStructureService.Update(chartStructureCreateOrUpdateDto);

    [HttpDelete]
    public  Task<bool> Delete(int id)
         => _chartStructureService.Delete(id);


}

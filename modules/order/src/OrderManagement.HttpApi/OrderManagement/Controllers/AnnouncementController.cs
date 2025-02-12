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
using OrderManagement.Domain;
using OrderManagement.Domain.Shared;
using IFG.Core.Utility.Tools;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/AnnouncementService/[action]")]
//[UserAuthorization]
public class AnnouncementController : Controller
{
    private readonly IAnnouncementService _announcementService;
    public AnnouncementController(IAnnouncementService announcementService)
        => _announcementService = announcementService;


    [HttpDelete]
    public Task<bool> Delete(int id)
    => _announcementService.Delete(id);

    //[HttpGet]
    //public async Task<AnnouncementDto> GetById(int id)
    //    => await _announcementService.GetById(id);

    [HttpGet]
    public Task<List<AnnouncementDto>> GetAllAnnouncement(AnnouncementDto input)
    => _announcementService.GetAllAnnouncement(input);
    [HttpGet]
    public  Task<AnnouncementDto> GetById(int id, string attachmentType, string attachmentlocation)
        => _announcementService.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));

    [HttpGet]
    public Task<PagedResultDto<AnnouncementDto>> GetPagination(AnnouncementGetListDto input)
    => _announcementService.GetPagination(input);
    [HttpPost]
    public Task<int> Insert(CreateAnnouncementDto announcementDto)
    => _announcementService.Insert(announcementDto);
    [HttpPut]
    public Task<int> Update(CreateAnnouncementDto announcementDto)
    => _announcementService.Update(announcementDto);
    [HttpPost]
    public Task<bool> UploadFile([FromForm] UploadFileDto uploadFile)
   => _announcementService.UploadFile(uploadFile);
    //public Task<AnnouncementDto> GetById(int id)
    //=> _announcementService.GetById(id);

    
}

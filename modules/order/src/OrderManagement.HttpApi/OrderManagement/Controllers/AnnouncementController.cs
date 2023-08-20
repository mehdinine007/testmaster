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
using OrderManagement.Domain;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/AnnouncementService/[action]")]
//[UserAuthorization]
public class AnnouncementController : Controller, IAnnouncementService
{
    private readonly IAnnouncementService _announcementService;
    public AnnouncementController(IAnnouncementService announcementService)
        => _announcementService = announcementService;


    [HttpDelete]
    public Task<bool> Delete(int id)
    => _announcementService.Delete(id);

    [HttpGet]
    public Task<List<AnnouncementDto>> GetAllAnnouncement()
    => _announcementService.GetAllAnnouncement();
    [HttpGet]
    public Task<PagedResultDto<AnnouncementDto>> GetList(AnnouncementGetListDto input)
    => _announcementService.GetList(input);
    [HttpPost]
    public Task<int> Add(CreateAnnouncementDto announcementDto)
    => _announcementService.Add(announcementDto);
    [HttpPut]
    public Task<int> Update(CreateAnnouncementDto announcementDto)
    => _announcementService.Update(announcementDto);
    [HttpPost]
    public Task<bool> UploadFile([FromForm]UploadFileDto uploadFile)
   => _announcementService.UploadFile(uploadFile);

    
}

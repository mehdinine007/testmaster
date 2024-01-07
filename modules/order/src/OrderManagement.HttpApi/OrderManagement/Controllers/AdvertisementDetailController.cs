using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;
using Volo.Abp.Application.Dtos;
using OrderManagement.Domain.Shared.OrderManagement.Enums;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/AdvertisementDetailService/[action]")]
    public class AdvertisementDetailController : Controller
    {
        private readonly IAdvertisementDetailService _advertisementDetailService;
        public AdvertisementDetailController(IAdvertisementDetailService advertisementDetailService)
            => _advertisementDetailService = advertisementDetailService;
        [HttpPost]
        public async Task<AdvertisementDetailDto> Add(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto)
        =>await _advertisementDetailService.Add(advertisementDetailCreateOrUpdateDto);

        [HttpDelete]
        public async Task<bool> Delete(AdvertisementDetailWithIdDto advertisementDetailWithId)
        =>await _advertisementDetailService.Delete(advertisementDetailWithId);
        [HttpGet]
        public async Task<AdvertisementDetailDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        =>await _advertisementDetailService.GetById(id, attachmentType, attachmentlocation);
        [HttpGet]
        public async Task<List<AdvertisementDetailDto>> GetList(int advertisementId,List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        =>await _advertisementDetailService.GetList(advertisementId,attachmentType, attachmentlocation);
        [HttpGet]
        public async Task<PagedResultDto<AdvertisementDetailDto>> GetPagination(AdvertisementDetailPaginationDto input)
       =>await _advertisementDetailService.GetPagination(input);
        [HttpPut]
        public async Task<AdvertisementDetailDto> Update(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto)
        => await _advertisementDetailService.Update(advertisementDetailCreateOrUpdateDto);
        [HttpPost]
        public async Task<Guid> UploadFile([FromForm]UploadFileDto uploadFile)
        => await _advertisementDetailService.UploadFile(uploadFile);
        [HttpGet]
        public async Task<bool> Move(AdvertisementDetailWithIdDto advertisementDetailWithId, MoveTypeEnum moveType)
       => await _advertisementDetailService.Move(advertisementDetailWithId, moveType);
    }
}

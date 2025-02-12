﻿using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;
using Volo.Abp.Application.Dtos;
using IFG.Core.Utility.Tools;

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
        public async Task<bool> Delete(int id)
        =>await _advertisementDetailService.Delete(id);
        [HttpGet]
        public async Task<AdvertisementDetailDto> GetById(int id, string attachmentType , string attachmentlocation )
        =>await _advertisementDetailService.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));
        [HttpGet]
        public async Task<List<AdvertisementDetailDto>> GetList(int advertisementId,string attachmentType ,string attachmentlocation )
        =>await _advertisementDetailService.GetList(advertisementId, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));
        [HttpGet]
        public async Task<PagedResultDto<AdvertisementDetailDto>> GetPagination(AdvertisementDetailPaginationDto input)
       =>await _advertisementDetailService.GetPagination(input);
        [HttpPut]
        public async Task<AdvertisementDetailDto> Update(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto)
        => await _advertisementDetailService.Update(advertisementDetailCreateOrUpdateDto);
        [HttpPost]
        public async Task<Guid> UploadFile([FromForm]UploadFileDto uploadFile)
        => await _advertisementDetailService.UploadFile(uploadFile);
        [HttpPut]
        public async Task<bool> Move(AdvertisementDetailWithIdDto advertisementDetailWithId)
       => await _advertisementDetailService.Move(advertisementDetailWithId);
    }
}

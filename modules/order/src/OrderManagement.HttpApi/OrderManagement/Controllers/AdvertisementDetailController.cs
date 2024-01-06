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
        public Task<AdvertisementDetailDto> Add(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto)
        => _advertisementDetailService.Add(advertisementDetailCreateOrUpdateDto);

        [HttpDelete]
        public Task<bool> Delete(int id)
        => _advertisementDetailService.Delete(id);
        [HttpGet]
        public Task<AdvertisementDetailDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        => _advertisementDetailService.GetById(id, attachmentType, attachmentlocation);
        [HttpGet]
        public Task<List<AdvertisementDetailDto>> GetList(int advertisementId,List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        => _advertisementDetailService.GetList(advertisementId,attachmentType, attachmentlocation);
        [HttpGet]
        public Task<PagedResultDto<AdvertisementDetailDto>> GetPagination(AdvertisementDetailPaginationDto input)
       => _advertisementDetailService.GetPagination(input);


        [HttpPut]
        public Task<AdvertisementDetailDto> Update(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto)
        => _advertisementDetailService.Update(advertisementDetailCreateOrUpdateDto);
        [HttpPost]
        public Task<Guid> UploadFile([FromForm]UploadFileDto uploadFile)
        => _advertisementDetailService.UploadFile(uploadFile);
    }
}

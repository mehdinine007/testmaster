using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.OrderManagement.Implementations;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;
using IFG.Core.Utility.Tools;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/AdvertisementService/[action]")]
    public class AdvertisementController: Controller
    {
        private readonly IAdvertisementService _advertisementService;
        public AdvertisementController(IAdvertisementService advertisementService)
            => _advertisementService = advertisementService;
        [HttpPost]
        public Task<AdvertisementDto> Add(AdvertisementCreateOrUpdateDto advertisementCreateOrUpdateDto)
        =>_advertisementService.Add(advertisementCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
       =>_advertisementService.Delete(id);  
        [HttpGet]
        public Task<AdvertisementDto> GetById(int id,string attachmentType = null, string attachmentlocation = null)
        => _advertisementService.GetById(new AdvertisementQueryDto {  Id =id,AttachmentType= attachmentType, Attachmentlocation= attachmentlocation });
        [HttpGet]
        public Task<List<AdvertisementDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        => _advertisementService.GetList(attachmentType, attachmentlocation);
        [HttpPut]
        public Task<AdvertisementDto> Update(AdvertisementCreateOrUpdateDto advertisementCreateOrUpdateDto)
        =>_advertisementService.Update(advertisementCreateOrUpdateDto);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement.Implementations;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts;
using OrderManagement.Domain.Shared;
using IFG.Core.Utility.Tools;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/BankService/[action]")]
    //[UserAuthorization]
    public class BankController: Controller
    {
        private readonly IBankAppService _bankAppService;
        public BankController(IBankAppService bankAppService)
        => _bankAppService = bankAppService;

        [HttpGet]
        public Task<BankDto> GetById(int id, string attachmentType, string attachmentlocation)
    => _bankAppService.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));


        [HttpGet]
        public Task<List<BankDto>> GetList(string attachmentType, string attachmentlocation)
        => _bankAppService.GetList(EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));


        [HttpPost]
        public Task<BankDto> Add(BankCreateOrUpdateDto bankCreateOrUpdateDto)
        => _bankAppService.Add(bankCreateOrUpdateDto);

        [HttpPut]
        public Task<BankDto> Update(BankCreateOrUpdateDto bankCreateOrUpdateDto)
        => _bankAppService.Update(bankCreateOrUpdateDto);

        [HttpDelete]
        public Task<bool> Delete(int id)
        => _bankAppService.Delete(id);

        [HttpPost]
        public Task<bool> UploadFile([FromForm] UploadFileDto uploadFile)
       => _bankAppService.UploadFile(uploadFile);
        [HttpPost]
        public async Task<bool> Move(MoveDto moveDto)
        =>await _bankAppService.Move(moveDto);

    }
}

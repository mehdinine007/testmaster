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

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/SaleSchemaService/[action]")]
//[UserAuthorization]
public class SaleSchemaController : Controller
{
    private readonly ISaleSchemaService _saleSchemaService;
    public SaleSchemaController(ISaleSchemaService saleSchemaService)
        => _saleSchemaService = saleSchemaService;


    [HttpDelete]
    public Task<bool> Delete(int id)
    => _saleSchemaService.Delete(id);

    
    [HttpGet]
    public Task<List<SaleSchemaDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType=null)
    => _saleSchemaService.GetList(attachmentType);

    [HttpGet]
    public Task<SaleSchemaDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null)
         => _saleSchemaService.GetById(id,attachmentType);

    [HttpPost]
    public Task<SaleSchemaDto> Add(CreateSaleSchemaDto saleSchemaDto)
    => _saleSchemaService.Add(saleSchemaDto);
    [HttpPut]
    public Task<SaleSchemaDto> Update(CreateSaleSchemaDto saleSchemaDto)
    => _saleSchemaService.Update(saleSchemaDto);
    [HttpPost]
    public Task<bool> UploadFile([FromForm]UploadFileDto uploadFile)
   => _saleSchemaService.UploadFile(uploadFile);
}

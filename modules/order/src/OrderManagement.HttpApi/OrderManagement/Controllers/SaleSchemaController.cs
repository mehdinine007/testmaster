using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;
using IFG.Core.Utility.Tools;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/SaleSchemaService/[action]")]
public class SaleSchemaController : Controller
{
    private readonly ISaleSchemaService _saleSchemaService;
    public SaleSchemaController(ISaleSchemaService saleSchemaService)
        => _saleSchemaService = saleSchemaService;


    [HttpDelete]
    public Task<bool> Delete(int id)
    => _saleSchemaService.Delete(id);


    [HttpGet]
    public Task<List<SaleSchemaDto>> GetList(string attachmentType, string attachmentlocation)
      => _saleSchemaService.GetList(EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));
    
    

    [HttpGet]
    public Task<SaleSchemaDto> GetById(int id, string attachmentType, string attachmentlocation)
         => _saleSchemaService.GetById(id, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));

    [HttpPost]
    public Task<SaleSchemaDto> Add(CreateSaleSchemaDto saleSchemaDto)
    => _saleSchemaService.Add(saleSchemaDto);
    [HttpPut]
    public Task<SaleSchemaDto> Update(CreateSaleSchemaDto saleSchemaDto)
    => _saleSchemaService.Update(saleSchemaDto);
    [HttpPost]
    public Task<bool> UploadFile([FromForm] UploadFileDto uploadFile)
   => _saleSchemaService.UploadFile(uploadFile);
}

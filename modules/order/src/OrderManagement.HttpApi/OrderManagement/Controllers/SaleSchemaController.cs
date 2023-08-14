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

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/SaleSchemaService/[action]")]
//[UserAuthorization]
public class SaleSchemaController : Controller, ISaleSchemaService
{
    private readonly ISaleSchemaService _saleSchemaService;
    public SaleSchemaController(ISaleSchemaService saleSchemaService)
        => _saleSchemaService = saleSchemaService;


    [HttpDelete]
    public Task<bool> Delete(int id)
    => _saleSchemaService.Delete(id);

    [HttpGet]
    public Task<List<SaleSchemaDto>> GetAllSaleSchema()
    => _saleSchemaService.GetAllSaleSchema();
    [HttpGet]
    public Task<PagedResultDto<SaleSchemaDto>> GetList(SaleSchemaGetListDto input)
    => _saleSchemaService.GetList(input);
    [HttpPost]
    public Task<int> Add(CreateSaleSchemaDto saleSchemaDto)
    => _saleSchemaService.Add(saleSchemaDto);
    [HttpPut]
    public Task<int> Update(CreateSaleSchemaDto saleSchemaDto)
    => _saleSchemaService.Update(saleSchemaDto);
    [HttpPost]
    public Task<bool> UploadFile([FromForm]UploadFileDto uploadFile)
   => _saleSchemaService.UploadFile(uploadFile);
}

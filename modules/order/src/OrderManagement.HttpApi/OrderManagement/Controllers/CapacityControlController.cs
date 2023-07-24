
using Esale.Share.Authorize;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement;
using OrderManagement.Application.OrderManagement.Implementations;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace OrderManagement.HttpApi;
[DisableAuditing]
[RemoteService]
[Route("api/services/app/CapacityControl/[action]")]
public class CapacityControlController :Controller
{
    private readonly ICapacityControlAppService _capacityControlAppService;
    private readonly IAttachmentService _attachmentService;
    public CapacityControlController(ICapacityControlAppService capacityControlAppService, IAttachmentService attachmentService)
    {
        _capacityControlAppService = capacityControlAppService;
        _attachmentService = attachmentService;
    }

    //[HttpPost]
    //public async Task<IResult> SaleDetails()
    //      =>  await _capacityControlAppService.SaleDetail();


    //[HttpPost]
    //public async Task<IResult> Payment()
    //      => await _capacityControlAppService.Payment();

    [HttpPost]
    public async Task GrpcPaymentTest()
          => await _capacityControlAppService.GrpcPaymentTest();

    [HttpPost]
    public async Task UploadFile([FromForm]AttachFileDto uploadFileDto)
          => await _attachmentService.UploadFile(uploadFileDto);

    [HttpPost]
    [UserAuthorization]
    public async Task<bool> Validation(Guid saleDetailUId)
          => await _capacityControlAppService.ValidationBySaleDetailUId(saleDetailUId);
}

using Esale.Core.DataAccess;
using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class SaleSchemaService : ApplicationService, ISaleSchemaService
{
    private readonly IRepository<SaleSchema> _saleSchemaRepository;
    private readonly IAttachmentService _attachmentService;
    public SaleSchemaService(IRepository<SaleSchema> saleSchemaRepository, IAttachmentService attachmentService)
    {
        _saleSchemaRepository = saleSchemaRepository;
        _attachmentService = attachmentService;
    }

    public async Task<bool> Delete(int id)
    {
        await Validation(id, null);
        await _saleSchemaRepository.DeleteAsync(x => x.Id == id);
        await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.SaleSchema, id);
        return true;
    }


    [SecuredOperation(SaleSchemaServicePermissionConstants.GetList)]
    public async Task<List<SaleSchemaDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType = null)
    {
        var count = _saleSchemaRepository.WithDetails().Count();
        var saleSchemas = (await _saleSchemaRepository.GetQueryableAsync()).ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.SaleSchema, saleSchemas.Select(x => x.Id).ToList(), attachmentType );
        var saleSchemaDto = ObjectMapper.Map<List<SaleSchema>, List<SaleSchemaDto>>(saleSchemas);
        saleSchemaDto.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return saleSchemaDto;
    }
    public async Task<SaleSchemaDto> Add(CreateSaleSchemaDto saleSchemaDto)
    {
        var saleSchema = ObjectMapper.Map<CreateSaleSchemaDto, SaleSchema>(saleSchemaDto);
        var entity = await _saleSchemaRepository.InsertAsync(saleSchema, autoSave: true);
        return ObjectMapper.Map<SaleSchema, SaleSchemaDto>(entity);
    }

    public async Task<SaleSchemaDto> Update(CreateSaleSchemaDto saleSchemaDto)
    {
        await Validation(saleSchemaDto.Id, null);
        var saleSchema = ObjectMapper.Map<CreateSaleSchemaDto, SaleSchema>(saleSchemaDto);
        await _saleSchemaRepository.AttachAsync(saleSchema, t => t.Title, d => d.Description, s => s.SaleStatus);
        return await GetById(saleSchemaDto.Id);
    }

    [SecuredOperation(SaleSchemaServicePermissionConstants.GetById)]
    public async Task<SaleSchemaDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null)
    {
        await Validation(id, null);
        var saleSchema = (await _saleSchemaRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == id);
        var saleSchemaDto = ObjectMapper.Map<SaleSchema, SaleSchemaDto>(saleSchema);

        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.SaleSchema, new List<int>() { id },attachmentType);
         saleSchemaDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
        return saleSchemaDto;

    }


    public async Task<bool> UploadFile(UploadFileDto uploadFile)
    {
        await Validation(uploadFile.Id, null);
        await _attachmentService.UploadFile(AttachmentEntityEnum.SaleSchema, uploadFile);
        return true;
    }

    private async Task<SaleSchema> Validation(int id, SaleSchemaDto saleSchemaDto)
    {
        var saleSchema = (await _saleSchemaRepository.GetQueryableAsync()).AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        if (saleSchema is null)
        {
            throw new UserFriendlyException(OrderConstant.SaleSchemaNotFound, OrderConstant.SaleSchemaFoundId);
        }
        return saleSchema;
    }


}

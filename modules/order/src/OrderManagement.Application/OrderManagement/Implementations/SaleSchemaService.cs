﻿using IFG.Core.DataAccess;
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
using Permission.Order;

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

    [SecuredOperation(SaleSchemaServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        await Validation(id, null);
        await _saleSchemaRepository.DeleteAsync(x => x.Id == id);
        await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.SaleSchema, id);
        return true;
    }


    [SecuredOperation(SaleSchemaServicePermissionConstants.GetList)]
    public async Task<List<SaleSchemaDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        var count = _saleSchemaRepository.WithDetails().Count();
        var saleSchemas = (await _saleSchemaRepository.GetQueryableAsync()).ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.SaleSchema, saleSchemas.Select(x => x.Id).ToList(), attachmentType, attachmentlocation);
        var saleSchemaDto = ObjectMapper.Map<List<SaleSchema>, List<SaleSchemaDto>>(saleSchemas);
        saleSchemaDto.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return saleSchemaDto;
    }

    [SecuredOperation(SaleSchemaServicePermissionConstants.Add)]
    public async Task<SaleSchemaDto> Add(CreateSaleSchemaDto saleSchemaDto)
    {
        var saleSchema = ObjectMapper.Map<CreateSaleSchemaDto, SaleSchema>(saleSchemaDto);
        var entity = await _saleSchemaRepository.InsertAsync(saleSchema);
        return ObjectMapper.Map<SaleSchema, SaleSchemaDto>(entity);
    }

    [SecuredOperation(SaleSchemaServicePermissionConstants.Update)]
    public async Task<SaleSchemaDto> Update(CreateSaleSchemaDto saleSchemaDto)
    {
        var saleSchemaEntity = await Validation(saleSchemaDto.Id, null);
        var saleSchema = ObjectMapper.Map<CreateSaleSchemaDto, SaleSchema>(saleSchemaDto, saleSchemaEntity);
        await _saleSchemaRepository.UpdateAsync(saleSchema);
        return await GetById(saleSchemaDto.Id);
    }

    [SecuredOperation(SaleSchemaServicePermissionConstants.GetById)]
    public async Task<SaleSchemaDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        await Validation(id, null);
        var saleSchema = (await _saleSchemaRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == id);
        var saleSchemaDto = ObjectMapper.Map<SaleSchema, SaleSchemaDto>(saleSchema);

        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.SaleSchema, new List<int>() { id }, attachmentType, attachmentlocation);
        saleSchemaDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
        return saleSchemaDto;

    }


    [SecuredOperation(SaleSchemaServicePermissionConstants.UploadFile)]
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

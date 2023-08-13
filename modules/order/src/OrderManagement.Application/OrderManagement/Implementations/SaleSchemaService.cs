using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
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
      var saleSchema= (await _saleSchemaRepository.GetQueryableAsync()).AsNoTracking().FirstOrDefault(x => x.Id == id);
        if (saleSchema is null)
        {
            throw new UserFriendlyException("شناسه وارد شده معتبر نمیباشد.");
        }
        await _saleSchemaRepository.DeleteAsync(x => x.Id == id);
        await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.SaleSchema, id);
        return true;
    }

    public async Task<List<SaleSchemaDto>> GetAllSaleSchema()
    {
        var saleSchema = await _saleSchemaRepository.GetListAsync();
        var saleSchemaDto = ObjectMapper.Map<List<SaleSchema>, List<SaleSchemaDto>>(saleSchema);
        return saleSchemaDto;
    }

    public async Task<PagedResultDto<SaleSchemaDto>> GetSaleSchema(SaleSchemaGetListDto input)
    {
        var count = _saleSchemaRepository.WithDetails().Count();
        var saleSchemaResult = await _saleSchemaRepository.GetQueryableAsync();
        var saleSchemaList = saleSchemaResult
            .Skip(input.SkipCount * input.MaxResultCount).Take(input.MaxResultCount)
            .AsNoTracking()
            .ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.SaleSchema, saleSchemaList.Select(x => x.Id).ToList(), input.AttachmentType);
        var saleSchema = ObjectMapper.Map<List<SaleSchema>, List<SaleSchemaDto>>(saleSchemaList);
        saleSchema.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return new PagedResultDto<SaleSchemaDto>
        {
            TotalCount = count,
            Items = saleSchema
        };

    }
    [UnitOfWork]
    public async Task<int> Save(CreateSaleSchemaDto saleSchemaDto)
    {
        var saleSchema = ObjectMapper.Map<CreateSaleSchemaDto, SaleSchema>(saleSchemaDto);
        await _saleSchemaRepository.InsertAsync(saleSchema, autoSave: true);
        return saleSchema.Id;
    }

    public async Task<int> Update(CreateSaleSchemaDto saleSchemaDto)
    {
        var getSaleSchema =(await _saleSchemaRepository.GetQueryableAsync()).AsNoTracking().FirstOrDefault(x=>x.Id== saleSchemaDto.Id);
        if (getSaleSchema is null)
        {
            throw new UserFriendlyException("شناسه وارد شده معتبر نمیباشد.");
        }
        var saleSchema = ObjectMapper.Map<CreateSaleSchemaDto, SaleSchema>(saleSchemaDto);
        await _saleSchemaRepository.AttachAsync(saleSchema, t => t.Title, d => d.Description, s => s.SaleStatus);
        return saleSchema.Id;
    }

    public async Task<bool> UploadFile(UploadFileDto uploadFile)
    {
        await _attachmentService.UploadFile(AttachmentEntityEnum.SaleSchema, uploadFile);
        return true;
    }




}

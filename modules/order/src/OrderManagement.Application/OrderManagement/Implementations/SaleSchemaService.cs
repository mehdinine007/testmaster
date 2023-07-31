using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        await _saleSchemaRepository.DeleteAsync(x => x.Id == id);
        return true;
    }

    public async Task<List<SaleSchemaDto>> GetAllSaleSchema()
    {
        var saleSchema = await _saleSchemaRepository.GetListAsync();
        var saleSchemaDto = ObjectMapper.Map<List<SaleSchema>, List<SaleSchemaDto>>(saleSchema);
        return saleSchemaDto;
    }

    public async Task<PagedResultDto<SaleSchemaDto>> GetSaleSchema(int pageNo, int sizeNo)
    {
        var count = _saleSchemaRepository.WithDetails().Count();
        var saleSchemaResult = await _saleSchemaRepository.GetQueryableAsync();
        var saleSchemaList = saleSchemaResult
            .Skip(pageNo * sizeNo).Take(sizeNo)
            .AsNoTracking()
            .ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.SaleSchema, saleSchemaList.Select(x => x.Id).ToList());
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
    public async Task<int> Save(SaleSchemaDto saleSchemaDto)
    {
        var saleSchema = ObjectMapper.Map<SaleSchemaDto, SaleSchema>(saleSchemaDto);
        await _saleSchemaRepository.InsertAsync(saleSchema, autoSave: true);
        return saleSchema.Id;
    }

    public async Task<int> Update(SaleSchemaDto saleSchemaDto)
    {
        var saleSchema = ObjectMapper.Map<SaleSchemaDto, SaleSchema>(saleSchemaDto);
        await _saleSchemaRepository.AttachAsync(saleSchema, t => t.Title, d => d.Description, s => s.SaleStatus);
        return saleSchema.Id;
    }

    public async Task<bool> UploadFile(UploadFileDto uploadFile)
    {
        await _attachmentService.UploadFile(new AttachFileDto()
        {
            Entity = AttachmentEntityEnum.SaleSchema,
            EntityId = uploadFile.Id,
            EntityType = uploadFile.Type,
            File = uploadFile.File,
        });

        return true;
    }

}

using Esale.Share.Authorize;
using IFG.Core.DataAccess;
using IFG.Core.Utility.Tools;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class OrganizationService : ApplicationService, IOrganizationService
{

    private readonly IRepository<Organization> _organizationRepository;
    private readonly IAttachmentService _attachmentService;



    public OrganizationService(IRepository<Organization> organizationRepository, IAttachmentService attachmentService)
    {
        _organizationRepository = organizationRepository;
        _attachmentService = attachmentService;
    }

    [SecuredOperation(OrganizationServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        var organ = (await _organizationRepository.GetQueryableAsync()).AsNoTracking().FirstOrDefault(x => x.Id == id);
        if (organ is null)
        {
            throw new UserFriendlyException("شناسه وارد شده معتبر نمیباشد.");
        }
        await _organizationRepository.DeleteAsync(organ);
        await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.Organization, id);
        return true;
    }

    [SecuredOperation(OrganizationServicePermissionConstants.GetAll)]
    public async Task<List<OrganizationDto>> GetAll()
    {
        var organ = await _organizationRepository.GetListAsync();
        var organdto = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(organ);
        return organdto;
    }   

    [SecuredOperation(OrganizationServicePermissionConstants.Save)]
    public async Task<int> Save(OrganizationInsertDto organDto)
    {

        var maxCode = await _organizationRepository.MaxAsync(o => o.Code);
        organDto.Code = ++maxCode;
        var organ = ObjectMapper.Map<OrganizationInsertDto, Organization>(organDto);
        await _organizationRepository.InsertAsync(organ, autoSave: true);
        return organ.Id;
    }

    [SecuredOperation(OrganizationServicePermissionConstants.Update)]
    public async Task<int> Update(OrganizationUpdateDto organDto)

    {
        var result = await _organizationRepository.WithDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == organDto.Id);
        if (result == null)
        {
            throw new UserFriendlyException("رکوردی برای ویرایش وجود ندارد");
        }
        var organ = ObjectMapper.Map<OrganizationUpdateDto, Organization>(organDto);
        await _organizationRepository.AttachAsync(organ, c=>c.Title,c=>c.Url, c => c.EncryptKey);

        return organ.Id;
    }

    //[SecuredOperation(OrganizationServicePermissionConstants.GetById)]
    public async Task<OrganizationDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        var organ = await Validation(id, null);
        var announcement = (await _organizationRepository.GetQueryableAsync()).AsNoTracking()
           .FirstOrDefault(x => x.Id == id)
           ?? throw new UserFriendlyException("شرکت مورد نظر یافت نشد");
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Organization, new List<int>() { id }, attachmentType, attachmentlocation);
        var organDto = ObjectMapper.Map<Organization, OrganizationDto>(organ);

        organDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
        return organDto;
    }


    [SecuredOperation(OrganizationServicePermissionConstants.UploadFile)]
    public async Task<bool> UploadFile(UploadFileDto uploadFile)
    {
        var announcement = await Validation(uploadFile.Id, null);
        await _attachmentService.UploadFile(AttachmentEntityEnum.Organization, uploadFile);
        return true;
    }


    private async Task<Organization> Validation(int id, OrganizationUpdateDto organDto)
    {
        var organ =(await  _organizationRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == id);
        if (organ is null)
        {
            throw new UserFriendlyException(OrderConstant.BankNotFound, OrderConstant.BankNotFoundId);
        }
        return organ;
    }
}

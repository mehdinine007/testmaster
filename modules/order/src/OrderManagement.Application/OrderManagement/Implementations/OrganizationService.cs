using Core.Utility.Tools;
using Esale.Share.Authorize;
using IFG.Core.DataAccess;
using IFG.Core.Utility.Tools;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
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

    private readonly IRepository<Organization, int> _organizationRepository;
    private readonly IAttachmentService _attachmentService;



    public OrganizationService(IRepository<Organization, int> organizationRepository, IAttachmentService attachmentService)
    {
        _organizationRepository = organizationRepository;
        _attachmentService = attachmentService;
    }

    [SecuredOperation(OrganizationServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        var Result = await Validation(id, null);
        await _organizationRepository.DeleteAsync(x => x.Id == id);
        await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.Organization, id);
        return true;
    }

    //[SecuredOperation(OrganizationServicePermissionConstants.GetAll)]
    public async Task<List<OrganizationDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        var organ = (await _organizationRepository.GetQueryableAsync()).AsNoTracking().ToList();
        var organdto = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(organ);
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Organization, organdto.Select(x => x.Id).ToList(), attachmentType, attachmentlocation);
        organdto.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return organdto;
    }

    [SecuredOperation(OrganizationServicePermissionConstants.Save)]
    public async Task<int> Add(OrganizationInsertDto organDto)
    {

        var organization = (await _organizationRepository.GetQueryableAsync()).AsNoTracking();
        var maxCode = await organization.MaxAsync(o => o.Code);
        var maxPriority = await organization.MaxAsync(o => o.Priority);

        var organ = ObjectMapper.Map<OrganizationInsertDto, Organization>(organDto);
        organ.Code = StringHelper.GenerateTreeNodePath(maxCode, null, 4);
        organ.Priority = maxPriority + 1;
        await _organizationRepository.InsertAsync(organ, autoSave: true);
        return organ.Id;

    }

    [SecuredOperation(OrganizationServicePermissionConstants.Update)]
    public async Task<int> Update(OrganizationUpdateDto organDto)
    {
        var _organ = await Validation(organDto.Id, null);
        var organ = ObjectMapper.Map<OrganizationUpdateDto, Organization>(organDto, _organ);
        await _organizationRepository.UpdateAsync(organ);
        return organ.Id;
    }

    //[SecuredOperation(OrganizationServicePermissionConstants.GetById)]
    public async Task<OrganizationDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        var organ = await Validation(id, null);
        var organDto = ObjectMapper.Map<Organization, OrganizationDto>(organ);
       
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Organization, new List<int>() { id }, attachmentType, attachmentlocation);

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
        var organ = (await _organizationRepository.GetQueryableAsync()).AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        if (organ is null)
        {
            throw new UserFriendlyException(OrderConstant.OrganizationNotFound, OrderConstant.OrganizationNotFoundId);
        }
        return organ;
    }
    [SecuredOperation(OrganizationServicePermissionConstants.Move)]
    public async Task<bool> Move(OrganizationPriorityDto input)
    {
        await Validation(input.Id, null);
        var organizationQuery = (await _organizationRepository.GetQueryableAsync()).AsNoTracking().OrderBy(x => x.Priority);
        var currentorganization = organizationQuery.FirstOrDefault(x => x.Id == input.Id);
        var currentPriority = currentorganization.Priority;

        if (MoveTypeEnum.Up == input.MoveType)
        {
            var previousorganization = await organizationQuery.OrderByDescending(x => x.Priority).FirstOrDefaultAsync(x => x.Priority < currentorganization.Priority);
            if (previousorganization is null)
            {
                throw new UserFriendlyException(OrderConstant.FirstPriority, OrderConstant.FirstPriorityId);
            }
            var previousPriority = previousorganization.Priority;

            currentorganization.Priority = previousPriority;

            await _organizationRepository.UpdateAsync(currentorganization);
            previousorganization.Priority = currentPriority;
            await _organizationRepository.UpdateAsync(previousorganization);
        }
        else if (MoveTypeEnum.Down == input.MoveType)
        {
            var nextorganization = await organizationQuery.FirstOrDefaultAsync(x => x.Priority > currentorganization.Priority);
            var orgId = await organizationQuery.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (nextorganization is null)
            {
                throw new UserFriendlyException(OrderConstant.LastPriority, OrderConstant.LastPriorityId);
            }
            var nextpriority = nextorganization.Priority;
            currentorganization.Priority = nextpriority;
            await _organizationRepository.UpdateAsync(currentorganization);
            nextorganization.Priority = currentPriority;
            await _organizationRepository.UpdateAsync(nextorganization);
        }
        return true;
    }
}

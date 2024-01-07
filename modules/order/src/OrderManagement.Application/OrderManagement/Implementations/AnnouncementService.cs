using Esale.Share.Authorize;
using IFG.Core.DataAccess;
using IFG.Core.Utility.Tools;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
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

public class AnnouncementService : ApplicationService, IAnnouncementService
{
    private readonly IRepository<Announcement> _announcementRepository;
    private readonly IAttachmentService _attachmentService;

    public AnnouncementService(IRepository<Announcement> announcementRepository, IAttachmentService attachmentService)
    {
        _announcementRepository = announcementRepository;
        _attachmentService = attachmentService;
    }
    [SecuredOperation(AnnouncementServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        var announcement = (await _announcementRepository.GetQueryableAsync()).AsNoTracking().FirstOrDefault(x => x.Id == id);
        if (announcement is null)
            throw new UserFriendlyException(OrderConstant.AnnouncementNotFound, OrderConstant.AnnouncementNotFoundId);
        await _announcementRepository.DeleteAsync(announcement);
        await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.Announcement, id);
        return true;
    }

    public async Task<List<AnnouncementDto>> GetAllAnnouncement(AnnouncementDto input)
    {
        var announcementRepository = await _announcementRepository.GetQueryableAsync();
        announcementRepository = announcementRepository.Where(x => x.Active);
        if (input.CompanyId.HasValue)
        {
            if (input.CompanyId != 0)
            {
                var announcementCompany = announcementRepository.Where(x => x.CompanyId == input.CompanyId).ToList();
                return await getAttachment(AttachmentEntityEnum.Announcement,
                     announcementCompany.Select(x => x.Id).ToList(),
                     EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.AttachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.AttachmentLocation), announcementCompany);
            }
            var announcementCompanies = announcementRepository.Where(x => x.CompanyId != null).ToList();
            return await getAttachment(AttachmentEntityEnum.Announcement,
                   announcementCompanies.Select(x => x.Id).ToList(),
                   EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.AttachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.AttachmentLocation), announcementCompanies);
        }

        var announcement = announcementRepository.Where(x => x.CompanyId == null).ToList();
        return await getAttachment(AttachmentEntityEnum.Announcement,
                 announcement.Select(x => x.Id).ToList(),
                 EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.AttachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.AttachmentLocation), announcement);
    }

    public async Task<List<AnnouncementDto>> getAttachment(AttachmentEntityEnum attachment, List<int> id, List<AttachmentEntityTypeEnum> typeEnum, List<AttachmentLocationEnum> attachmentlocation, List<Announcement> announcementCompany)
    {
        var attachments = await _attachmentService.GetList(attachment, id, typeEnum, attachmentlocation);

        var result = ObjectMapper.Map<List<Announcement>, List<AnnouncementDto>>(announcementCompany);
        result.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return result;
    }

    public async Task<PagedResultDto<AnnouncementDto>> GetPagination(AnnouncementGetListDto input)
    {
        var announcementResult = await _announcementRepository.GetQueryableAsync();
        if (input.CompanyId.HasValue)
            announcementResult = announcementResult.Where(x => x.CompanyId.Value == input.CompanyId.Value);
        if (input.Active.HasValue)
            announcementResult = announcementResult.Where(x => x.Active == input.Active.Value);
        var count = announcementResult.Count();
        var announcementList = announcementResult
            .AsNoTracking()
            .SortByRule(input)
            .PageBy(input)
            .ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Announcement, announcementList.Select(x => x.Id).ToList()
                                                    , EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.AttachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.AttachmentLocation));
        var announcement = ObjectMapper.Map<List<Announcement>, List<AnnouncementDto>>(announcementList);
        announcement.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return new PagedResultDto<AnnouncementDto>
        {
            TotalCount = count,
            Items = announcement
        };

    }
    //public async Task<List<AnnouncementDto>> GetList(AttachmentEntityTypeEnum? attachmentType)
    //{
    //    var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Announcement, announcementList.Select(x => x.Id).ToList(), input.AttachmentType);

    //    var announcement = ObjectMapper.Map<List<Announcement>, List<AnnouncementDto>>(announcementList);
    //    announcement.ForEach(x =>
    //    {
    //        var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
    //        x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
    //    });
    //    return announcement;
    //}
    [UnitOfWork]
    [SecuredOperation(AnnouncementServicePermissionConstants.Add)]
    public async Task<int> Insert(CreateAnnouncementDto announcementDto)
    {
        if(announcementDto.ToDate < announcementDto.FromDate)
            throw new UserFriendlyException(OrderConstant.ToDateLessThanFromDate, OrderConstant.ToDateLessThanFromDateId);
        var announcement = ObjectMapper.Map<CreateAnnouncementDto, Announcement>(announcementDto);
        await _announcementRepository.InsertAsync(announcement);
        await CurrentUnitOfWork.SaveChangesAsync();
        return announcement.Id;

    }

    [SecuredOperation(AnnouncementServicePermissionConstants.Update)]
    public async Task<int> Update(CreateAnnouncementDto announcementDto)
    {
        var getAnnouncement = (await _announcementRepository.GetQueryableAsync()).AsNoTracking().FirstOrDefault(x => x.Id == announcementDto.Id);
        if (getAnnouncement is null)
            throw new UserFriendlyException(OrderConstant.AnnouncementNotFound, OrderConstant.AnnouncementNotFoundId);
      
        if (announcementDto.ToDate < announcementDto.FromDate)
            throw new UserFriendlyException(OrderConstant.ToDateLessThanFromDate, OrderConstant.ToDateLessThanFromDateId);
        var announcement =ObjectMapper.Map<CreateAnnouncementDto, Announcement>(announcementDto, getAnnouncement);
        await _announcementRepository.UpdateAsync(announcement);
        await CurrentUnitOfWork.SaveChangesAsync();
        return announcement.Id;
    }

    [SecuredOperation(AnnouncementServicePermissionConstants.UploadFile)]
    public async Task<bool> UploadFile(UploadFileDto uploadFile)
    {
        var announcement = await Validation(uploadFile.Id, null);
        await _attachmentService.UploadFile(AttachmentEntityEnum.Announcement, uploadFile);
        return true;
    }

    public async Task<AnnouncementDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        var announc = await Validation(id, null);
        var announcement = (await _announcementRepository.GetQueryableAsync()).AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        var announcementDto = ObjectMapper.Map<Announcement, AnnouncementDto>(announcement);
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Announcement, new List<int>() { id }, attachmentType, attachmentlocation);

        announcementDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);

        return announcementDto;
    }


    private async Task<Announcement> Validation(int id, AnnouncementDto announcementDto)
    {
        var announcement = (await _announcementRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == id);
        if (announcement is null)
            throw new UserFriendlyException(OrderConstant.AnnouncementNotFound, OrderConstant.AnnouncementNotFoundId);
       
        return announcement;
    }

}

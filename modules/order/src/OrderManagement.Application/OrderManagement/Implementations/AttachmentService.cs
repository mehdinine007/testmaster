﻿using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using Permission.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class AttachmentService : ApplicationService, IAttachmentService
    {
        private readonly IRepository<Attachment> _attachementRepository;
        private IConfiguration _configuration { get; set; }
        public AttachmentService(IRepository<Attachment> attachementRepository, IConfiguration configuration)
        {
            _attachementRepository = attachementRepository;
            _configuration = configuration;
        }

        private async Task<Guid> Add(Attachment attachmentDto)
        {
            await Validation(null, attachmentDto);
            var iqAttachment = await _attachementRepository.GetQueryableAsync();
            if (attachmentDto.Priority == 0)
            {
                var attachment = iqAttachment
                    .Where(x => x.Entity == attachmentDto.Entity && x.EntityId == attachmentDto.EntityId && x.EntityType == attachmentDto.EntityType)
                    .ToList();
                var _priroity = 1;
                if (attachment != null && attachment.Count > 1)
                    _priroity = attachment.Max(x => x.Priority) + 1;
                attachmentDto.Priority = _priroity;
               
            }
            await _attachementRepository.InsertAsync(attachmentDto);
            return attachmentDto.Id;
        }

        [SecuredOperation(AttachmentServicePermissionConstants.Update)]
        public async Task<Guid> Update(AttachmentUpdateDto attachmentDto)
        {
            var attachment = await Validation(attachmentDto.Id, ObjectMapper.Map<AttachmentUpdateDto, Attachment>(attachmentDto));
            if (attachmentDto.File != null)
            {
                var attachFileDto = ObjectMapper.Map<Attachment, AttachFileDto>(ObjectMapper.Map(attachmentDto, attachment));
                attachFileDto.File = attachmentDto.File;
                attachment = CopyFile(attachFileDto);
            }
            await _attachementRepository.UpdateAsync(attachment);
            return attachment.Id;
        }
        [SecuredOperation(AttachmentServicePermissionConstants.Delete)]
        public async Task<bool> DeleteById(Guid id)
        {
            var attachment = await Validation(id, null);
            await _attachementRepository.DeleteAsync(x => x.Id == id);
            var filePath = _configuration.GetSection("Attachment:UploadFilePath").Value + "\\" + attachment.Id + "." + attachment.FileExtension;
            File.Delete(Path.Combine(filePath));
            return true;
        }
        private async Task<Attachment> Validation(Guid? id, Attachment attachmentDto)
        {
            var attachment = new Attachment();
            if (id != null)
            {
                attachment = (await _attachementRepository.GetQueryableAsync())
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == id);
                if (attachment is null)
                {
                    throw new UserFriendlyException(OrderConstant.AttachmentNotFound, OrderConstant.AttachmentNotFoundId);
                }
                if (attachmentDto != null)
                {
                    if (attachmentDto.Priority <= 0)
                    {
                        throw new UserFriendlyException(OrderConstant.AttachmentPriorityIsEmpty, OrderConstant.AttachmentPriorityIsEmptyId);
                    }
                    attachmentDto.Entity = attachment.Entity;
                    attachmentDto.EntityId = attachment.EntityId;
                }

            }
            if (attachmentDto != null)
            {
                var attachmentPriority = (await _attachementRepository.GetQueryableAsync())
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id != id && x.Entity == attachmentDto.Entity && x.EntityId == attachmentDto.EntityId && x.EntityType == attachmentDto.EntityType && x.Device == attachmentDto.Device);
                if (attachmentPriority != null && attachmentDto.EntityType != AttachmentEntityTypeEnum.Gallery)
                {
                    throw new UserFriendlyException(OrderConstant.AttachmentDuplicate, OrderConstant.AttachmentDuplicateId);
                }

                attachmentPriority = (await _attachementRepository.GetQueryableAsync())
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id != id && x.Priority == attachmentDto.Priority && x.Entity == attachmentDto.Entity && x.EntityId == attachmentDto.EntityId && x.EntityType == attachmentDto.EntityType && x.Device == attachmentDto.Device);
                if (attachmentPriority != null)
                {
                    throw new UserFriendlyException(OrderConstant.AttachmentPriorityDuplicate, OrderConstant.AttachmentPriorityDuplicateId);
                }
            }
            return attachment;
        }

        [SecuredOperation(AttachmentServicePermissionConstants.UploadFile)]
        public async Task<Guid> UploadFile(AttachmentEntityEnum entity, UploadFileDto uploadFile)
        {
            if (uploadFile.Id <= 0)
            {
                throw new UserFriendlyException(OrderConstant.AttachmentEntityIdNotFound, OrderConstant.AttachmentEntityIdNotFoundId);
            }
            var attachDto = ObjectMapper.Map<UploadFileDto, AttachFileDto>(uploadFile);
            attachDto.AttachmentId = Guid.NewGuid();
            attachDto.Entity = entity;
            attachDto.EntityId = uploadFile.Id;
            var attachment = CopyFile(attachDto);
            return await Add(attachment);
        }

        private Attachment CopyFile(AttachFileDto attachDto)
        {
            if (attachDto.File is null)
                throw new UserFriendlyException(OrderConstant.FileUploadNotFound, OrderConstant.FileUploadNotFoundId);
            string fileExtention = Path.GetExtension(attachDto.File.FileName);
            if (string.IsNullOrEmpty(fileExtention))
                throw new UserFriendlyException(OrderConstant.FileUploadNotExtention, OrderConstant.FileUploadNotExtentionId);
            string basePath = _configuration.GetSection("Attachment:UploadFilePath").Value;
            if (string.IsNullOrEmpty(basePath))
                throw new UserFriendlyException(OrderConstant.FileUploadNotPathNotExists, OrderConstant.FileUploadNotPathNotExistsId);
            var attachment = ObjectMapper.Map<AttachFileDto, Attachment>(attachDto);
            attachment.FileExtension = fileExtention.Replace(".", "");
            attachment.Content = JsonConvert.SerializeObject(attachDto.Content);
            attachment.VersionNumber++;
            string fileName = attachDto.AttachmentId.ToString()+ "." + attachment.FileExtension;
            var files = Directory.EnumerateFiles(basePath).Where(f => f.Contains(attachDto.AttachmentId.ToString()));
            foreach (var file in files)
            {
                File.Delete(file);
            }
            string filePath = Path.Combine(basePath, fileName);
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                attachDto.File.CopyTo(stream);
            }
            return attachment;
        }

        public async Task<List<AttachmentDto>> GetList(AttachmentEntityEnum entity, List<int> entityIds, List<AttachmentEntityTypeEnum> entityType=null, List<AttachmentLocationEnum> location = null)
        {
            var iqAttachment = await _attachementRepository.GetQueryableAsync();
            if (entityType is null || entityType.Count==0)
                iqAttachment = iqAttachment
                    .Where(x => x.Entity == entity && entityIds.Contains(x.EntityId) && (location == null || location.Contains(x.Location)));
            else
                iqAttachment = iqAttachment
                   .Where(x => x.Entity == entity && entityType.Contains(x.EntityType) && entityIds.Contains(x.EntityId) && (location == null || location.Contains(x.Location)));
            var attachments = iqAttachment
                .OrderBy(x => x.Priority)
                .ToList();
            return ObjectMapper.Map<List<Attachment>, List<AttachmentDto>>(attachments);
        }

        public async Task<bool> DeleteByEntityId(AttachmentEntityEnum entity, int id)
        {
            var attachments = (await _attachementRepository.GetQueryableAsync())
                 .Where(x => x.Entity == entity && x.EntityId == id)
                 .ToList();
            if (attachments == null || attachments.Count == 0)
                return true;
            foreach (var attachment in attachments)
            {
                await DeleteById(attachment.Id);
            }
            return true;
        }

    }
}
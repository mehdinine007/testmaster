using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
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
            var iqAttachment = await _attachementRepository.GetQueryableAsync();
            var attachment = iqAttachment
                .Where(x => x.Entity == attachmentDto.Entity && x.EntityId == attachmentDto.EntityId && x.EntityType == attachmentDto.EntityType)
                .ToList();
            var _priroity = 1;
            if (attachment != null && attachment.Count > 1)
                _priroity = attachment.Max(x => x.Priority) + 1;
            attachmentDto.Priority = _priroity;
            await _attachementRepository.InsertAsync(attachmentDto, autoSave: true);
            return attachmentDto.Id;
        }

        private async Task<Guid> Update(Attachment attachmentDto)
        {
            var attachment = await Validation(attachmentDto.Id, attachmentDto);
            attachment.Title = attachmentDto.Title;
            await _attachementRepository.UpdateAsync(attachment, autoSave: true);
            return attachment.Id;
        }
        private async Task<bool> Delete(Guid id)
        {
            await Validation(id, null);
            await _attachementRepository.DeleteAsync(x => x.Id == id);
            return true;
        }
        private async Task<Attachment> Validation(Guid id, Attachment attachmentDto)
        {
            var attachment = await _attachementRepository
                .WithDetails()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (attachment is null)
            {
                throw new UserFriendlyException(OrderConstant.AttachmentNotFound, OrderConstant.AttachmentNotFoundId);
            }
            return attachment;
        }

        public async Task<bool> UploadFile(AttachmentEntityEnum entity, UploadFileDto uploadFile)
        {
            if (uploadFile.Id<=0)
            {
                throw new UserFriendlyException("شناسه وارد شده معتبر نمیباشد.");
            }

            if (uploadFile.File is null)
            {
                throw new UserFriendlyException("پرکردن فایل ضروری میباشد");
            }

            
            var attachDto = new AttachFileDto()
            {
                Entity = entity,
                EntityId = uploadFile.Id,
                EntityType = uploadFile.Type,
                File = uploadFile.File,
                Description = uploadFile.Description,
                Title = uploadFile.Title,
                Content = uploadFile.Content,
                Location = uploadFile.Location,
                Priority = uploadFile.Priority
            };
            bool hasAdd = attachDto.Id is null;
            if (hasAdd)
                attachDto.Id = Guid.NewGuid();
            var attachment = CopyFile(attachDto);
            if (hasAdd)
                await Add(attachment);
            else
                await Update(attachment);
            return true;
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
            string fileName = attachDto.Id.ToString() + "." + attachment.FileExtension;
            string filePath = Path.Combine(basePath, fileName);
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                attachDto.File.CopyTo(stream);
            }
            return attachment;
        }

        public async Task<List<AttachmentDto>> GetList(AttachmentEntityEnum entity, List<int> entityIds, AttachmentEntityTypeEnum? entityType = null)
        {
            var iqAttachment = await _attachementRepository.GetQueryableAsync();
            if (entityType is null)
                iqAttachment = iqAttachment
                    .Where(x => x.Entity == entity && entityIds.Contains(x.EntityId));
            else
                iqAttachment = iqAttachment
                    .Where(x => x.Entity == entity && x.EntityType == entityType && entityIds.Contains(x.EntityId));

            var attachments = iqAttachment.ToList();
            return ObjectMapper.Map<List<Attachment>, List<AttachmentDto>>(attachments);
        }
    }
}
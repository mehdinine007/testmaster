using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Contracts;
using OrderManagement.Domain.OrderManagement;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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
            await _attachementRepository.InsertAsync(attachmentDto, autoSave: true);
            return attachmentDto.Id;
        }

        private async Task<Guid> Update( Attachment attachmentDto)
        {
            var attachment = await Validation(attachmentDto.Id,attachmentDto);
            attachment.Title = attachmentDto.Title;
            await _attachementRepository.UpdateAsync(attachment, autoSave: true);
            return attachment.Id;
        }
        private async Task<bool> Delete(Guid id)
        {
            await Validation(id,null);
            await _attachementRepository.DeleteAsync(x => x.Id == id);
            return true;
        }
        private async Task<Attachment> Validation(Guid id,Attachment attachmentDto)
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

        public async Task<bool> UploadFile(AttachFileDto attachDto)
        {
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
            var attachment = ObjectMapper.Map<AttachmentDto, Attachment>(attachDto);
            attachment.FileExtension = fileExtention.Replace(".","");
            string fileName = attachDto.Id.ToString() + "." + attachment.FileExtension;
            string filePath = Path.Combine(basePath, fileName);
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                attachDto.File.CopyTo(stream);
            }
            return attachment;
        }
    }
}

using Nest;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class SiteStructureService : ApplicationService, ISiteStructureService
    {
        private readonly IRepository<SiteStructure, int> _siteStructureRepository;
        private readonly IAttachmentService _attachmentService;
        public SiteStructureService(IRepository<SiteStructure, int> siteStructureRepository, IAttachmentService attachmentService)
        {
            _siteStructureRepository = siteStructureRepository;
            _attachmentService = attachmentService;
        }
        public async Task<SiteStructureDto> Add(SiteStructureAddOrUpdateDto siteStructureDto)
        {
            var siteStructureQuery = await _siteStructureRepository.GetQueryableAsync();
            var siteStructure = siteStructureQuery
                .ToList();
            var _code = 1;
            if (siteStructure != null && siteStructure.Count > 1)
                _code = siteStructure.Max(x => x.Code) + 1;
            var _siteStructure = ObjectMapper.Map<SiteStructureAddOrUpdateDto, SiteStructure>(siteStructureDto);
            _siteStructure.Code = _code;
            _siteStructure = await _siteStructureRepository.InsertAsync(_siteStructure, autoSave: true);
            return await GetById(_siteStructure.Id);
        }
        public async Task<SiteStructureDto> Update(SiteStructureAddOrUpdateDto siteStructureDto)
        {
            var siteStructure = await Validation(siteStructureDto.Id, siteStructureDto);
            siteStructure.Title = siteStructureDto.Title;
            siteStructure.Description = siteStructureDto.Description;
            siteStructure.Type = siteStructureDto.Type;
            await _siteStructureRepository.UpdateAsync(siteStructure, autoSave: true);
            return await GetById(siteStructure.Id);
        }

        public async Task<bool> Delete(int id)
        {
            await Validation(id, null);
            await _siteStructureRepository.DeleteAsync(x => x.Id == id);
            await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.SiteStructure, id);
            return true;
        }

        private async Task<SiteStructure> Validation(int id, SiteStructureAddOrUpdateDto siteStructureDto)
        {
            var siteStructure = (await _siteStructureRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == id);
            if (siteStructure is null)
            {
                throw new UserFriendlyException(OrderConstant.SiteStuctureNotFound, OrderConstant.SiteStuctureNotFoundId);
            }
            return siteStructure;
        }


        public async Task<SiteStructureDto> GetByCode(int code)
        {
            var siteStructure = (await _siteStructureRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Code == code);
            return await GetById(siteStructure.Id);
        }

        public  async Task<SiteStructureDto> GetById(int id)
        {
            var siteStructure = (await _siteStructureRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == id);
            return ObjectMapper.Map<SiteStructure, SiteStructureDto>(siteStructure);
        }

        public async Task<List<SiteStructureDto>> GetList(AttachmentEntityTypeEnum? attachmentType)
        {
            var getSiteStructures = (await _siteStructureRepository.GetQueryableAsync())
                .ToList();
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.SiteStructure, getSiteStructures.Select(x => x.Id).ToList(), attachmentType);
            var siteStructures = ObjectMapper.Map<List<SiteStructure>, List<SiteStructureDto>>(getSiteStructures);
            siteStructures.ForEach(x =>
            {
                var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
            });
            return siteStructures;
        }

        public async Task<bool> UploadFile(UploadFileDto uploadFile)
        {
            await _attachmentService.UploadFile(AttachmentEntityEnum.SiteStructure, uploadFile);
            return true;
        }
    }
}

using MongoDB.Driver;
using Nest;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class ChartStructureService : ApplicationService, IChartStructureService
    {
        private readonly IRepository<ChartStructure, int> _chartStructureRepository;
        private readonly IAttachmentService _attachmentService;
        public ChartStructureService(IRepository<ChartStructure, int> chartStructureRepository, IAttachmentService attachmentService)
        {
            _chartStructureRepository = chartStructureRepository;
            _attachmentService = attachmentService;
        }
        public async Task<List<ChartStructureDto>> GetList(AttachmentEntityTypeEnum? attachmentType)
        {
            var chartStructures = (await _chartStructureRepository.GetQueryableAsync())
                .OrderBy(x => x.Priority)
                .ToList();
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ChartStructure, chartStructures.Select(x => x.Id).ToList(), attachmentType);
            var chartStructuresDto= ObjectMapper.Map<List<ChartStructure>, List<ChartStructureDto>>(chartStructures);
            chartStructuresDto.ForEach(x =>
            {
                var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
            });
            return chartStructuresDto;  
        }

        private async Task<ChartStructure> Validation(int id)
        {
            var chartStructure = (await _chartStructureRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == id);
            if (chartStructure is null)
            {
                throw new UserFriendlyException(OrderConstant.ChartStructureNotFound, OrderConstant.ChartStructureNotFoundId);
            }
            return chartStructure;
        }
        public async Task<bool> UploadFile(UploadFileDto uploadFile)
        {
            var chartStructure=await Validation(uploadFile.Id);
            await _attachmentService.UploadFile(AttachmentEntityEnum.ChartStructure, uploadFile);
            return true;
        }

        public async Task<ChartStructureDto> GetById(int id)
        {
            var chartStructure = await Validation(id);
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ChartStructure, new List<int>() { id });
            var chartStructureDto = ObjectMapper.Map<ChartStructure, ChartStructureDto>(chartStructure);
            chartStructureDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
            return chartStructureDto;
        }

    }
}

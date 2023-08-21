using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class ChartStructureService : ApplicationService, IChartStructureService
    {
        private readonly IRepository<ChartStructure, int> _chartStructureRepository;
        public ChartStructureService(IRepository<ChartStructure, int> chartStructureRepository)
        {
            _chartStructureRepository = chartStructureRepository;
        }
        public async Task<List<ChartStructureDto>> GetList()
        {
            var chartStructures = (await _chartStructureRepository.GetQueryableAsync())
                .OrderBy(x => x.Priority)
                .ToList();
            return ObjectMapper.Map<List<ChartStructure>, List<ChartStructureDto>>(chartStructures);
        }
    }
}

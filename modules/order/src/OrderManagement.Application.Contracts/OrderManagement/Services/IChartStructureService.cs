using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IChartStructureService: IApplicationService
    {
        Task<List<ChartStructureDto>> GetList();
    }
}

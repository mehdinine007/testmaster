using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface ISchemeService: IApplicationService
    {
        Task<SchemeDto> GetById(int id);
        Task<SchemeDto> Add(SchemeCreateOrUpdateDto schemeCreateOrUpdateDto);
        Task<SchemeDto> Update(SchemeCreateOrUpdateDto schemeCreateOrUpdateDto);
        Task<List<SchemeDto>> GetList();
        Task<bool> Delete(int id);
    }
}

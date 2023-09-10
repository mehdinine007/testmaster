using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IActivityService: IApplicationService
    {

        Task<ActivityDto> GetById(int? id);
        Task<ActivityDto> Add(ActivityCreateOrUpdateDto activityCreateOrUpdateDto);
        Task<ActivityDto> Update(ActivityCreateOrUpdateDto activityCreateOrUpdateDto);
        Task<List<ActivityDto>> GetList();
        Task<bool> Delete(int id);
        Task<ActivityDto> GetBySchemeId(int schemeId);
        Task<ActivityDto> NextAcvtivity(int id);

    }
}

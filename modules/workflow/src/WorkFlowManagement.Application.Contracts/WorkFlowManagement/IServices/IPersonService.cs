using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IPersonService: IApplicationService
    {

        Task<PersonDto> GetById(Guid id);
        Task<PersonDto> Add(PersonCreateOrUpdateDto personCreateOrUpdateDto);
        Task<PersonDto> Update(PersonCreateOrUpdateDto personCreateOrUpdateDto);
        Task<List<PersonDto>> GetList(int activityId);
        Task<bool> Delete(Guid id);
     

    }
}

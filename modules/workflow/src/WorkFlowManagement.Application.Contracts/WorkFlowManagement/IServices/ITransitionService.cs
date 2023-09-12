using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface ITransitionService: IApplicationService
    {

        Task<TransitionDto> GetById(int id);
        Task<TransitionDto> Add(TransitionCreateOrUpdateDto transitionCreateOrUpdateDto);
        Task<TransitionDto> Update(TransitionCreateOrUpdateDto transitionCreateOrUpdateDto);
        Task<List<TransitionDto>> GetList();
        Task<bool> Delete(int id);
    }
}

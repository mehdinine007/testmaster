using OrderManagement.Application.Contracts.Dtos;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IQuestionGroupService : IApplicationService
    {
        Task<List<QuestionGroupDto>> GetAll();
        Task<QuestionGroupDto> GetById(int Id);
        Task<QuestionGroupDto> Add(QuestionGroupDto questionGroup);
        Task<QuestionGroupDto> Update(QuestionGroupDto questionGroup);
        Task<bool> Delete(int Id);
    }
}

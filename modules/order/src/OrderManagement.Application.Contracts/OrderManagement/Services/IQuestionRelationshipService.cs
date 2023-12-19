using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IQuestionRelationshipService : IApplicationService
    {
        Task<QuestionRelationshipDto> GetById(int id);
        Task<List<QuestionRelationshipDto>> GetList(int QuestionId);
        Task<bool> Delete(int id);
        Task<int> Add(QuestionRelationshipDto questionRelationshipDto);
 
        Task<int> Update(QuestionRelationshipDto questionRelationshipDto);
    }
}

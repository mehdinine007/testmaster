using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IQuestionniareService : IApplicationService
    {
        Task<QuestionnaireDto> CreateQuestionnaire(QuestionnaireDto questionnaireDto);

        Task<List<AnswerComponentTypeDto>> GetAnswerComponentTypes();

        Task CreatQuestionnaireAnswers(List<QuestionnaireAnswerDto> questionnaireAnswers);

        //Task<QuestionnaireTree> GetQuestionnaireTree(int questionnaireId);

        Task<SubmiteAnswerDto> SubmitAnswer(SubmiteAnswerDto submiteAnswerDto);

        Task<List<QuestionnaireTree>> GetQuestionnaireTrees();
    }
}

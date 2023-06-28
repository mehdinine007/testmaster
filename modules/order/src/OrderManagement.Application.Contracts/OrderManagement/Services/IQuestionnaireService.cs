using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IQuestionnaireService : IApplicationService
    {
        Task<QuestionnaireDto> CreateQuestionnaire(QuestionnaireDto questionnaireDto);

        Task<List<AnswerComponentTypeDto>> GetAnswerComponentTypes();

        Task CreatQuestionnaireAnswers(CreateQuestionnaireAnswerDto questionnaireAnswers);

        //Task<QuestionnaireTree> GetQuestionnaireTree(int questionnaireId);

        Task<SubmitteAnswerDto> SubmitAnswer(SubmitteAnswerDto submitteAnswerDto);

        Task<List<QuestionnaireTree>> GetQuestionnaireTrees();

        Task SubmitAnswers(List<int> answerIds);
    }
}

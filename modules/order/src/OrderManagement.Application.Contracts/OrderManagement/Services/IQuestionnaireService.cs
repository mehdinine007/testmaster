using OrderManagement.Application.Contracts.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IQuestionnaireService : IApplicationService
    {
        public Task<QuestionnaireTreeDto> LoadQuestionnaireTree(int questionnaireId);

        public Task SubmitAnswer(SubmitAnswerTreeDto submitAnswerTreeDto);

        Task<List<QuestionnaireAnalysisDto>> GetQuestionnaireReport(int questionnaireId);
    }
}

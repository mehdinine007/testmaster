
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using Volo.Abp.Application.Services;

namespace AdminPanelManagement.Application.Contracts.IServices
{
    public interface IReportService : IApplicationService
    {
        public Task<List<ReportQuestionnaireDto>> ReportQuestionnaire(ReportQueryDto input);
    }
}

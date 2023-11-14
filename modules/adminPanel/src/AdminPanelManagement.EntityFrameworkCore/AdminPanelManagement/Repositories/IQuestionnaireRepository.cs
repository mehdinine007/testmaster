using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.EntityFrameworkCore.AdminPanelManagement.Repositories
{
    public interface IQuestionnaireRepository
    {
        public Task<List<ReportQuestionnaireDb>> GetReportQuestionnaire (string nationalCode, ReportQuestionnaireTypeEnum type, int? maxResultCount, int? skipCount);
    }
}

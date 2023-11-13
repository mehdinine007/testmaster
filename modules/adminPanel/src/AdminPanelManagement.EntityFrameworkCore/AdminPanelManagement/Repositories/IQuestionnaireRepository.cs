using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.EntityFrameworkCore.AdminPanelManagement.Repositories
{
    public interface IQuestionnaireRepository
    {
        public Task<List<ReportQuestionnaireDb>> GetReportQuestionnaire (string uid,int type, int? maxResultCount, int? skipCount);
    }
}

using AdminPanelManagement.Domain.AdminPanelManagement;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AdminPanelManagement.EntityFrameworkCore.AdminPanelManagement.Repositories
{
    public class QuestionnaireRepository : EfCoreRepository<AdminPanelManagementDbContext, Test, int>, IQuestionnaireRepository
    {
        private readonly IConfiguration _configuration;
        private string _connectionStrings;
        public QuestionnaireRepository(IConfiguration configuration, IDbContextProvider<AdminPanelManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _configuration = configuration;
            _connectionStrings = _configuration.GetConnectionString("AdminPanelManagement");

        }

        public async Task<List<ReportQuestionnaireDb>> GetReportQuestionnaire(string userId, ReportQuestionnaireTypeEnum type, int? maxResultCount, int?  skipCount)
        {
            var _dbContext = await GetDbContextAsync();
            var questionnaireParameters = new[] {
                new SqlParameter("@nationalCode", SqlDbType.NVarChar,10,null) { Direction = ParameterDirection.Input, Value = userId },
                new SqlParameter("@type", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = type },
                new SqlParameter("@maxResultCount", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = maxResultCount },
                new SqlParameter("@skipCount", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = skipCount },
            };
            var questionnaire = await _dbContext.Set<ReportQuestionnaireDb>().FromSqlRaw(string.Format("EXEC {0} {1}", "spGetReportQuestionnaire", "@nationalCode,@type,@maxResultCount,@skipCount"), questionnaireParameters).ToListAsync();
            return questionnaire;
        }
    }
}

using Dapper;
using EasyCaching.Core.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ReportManagement.Domain.ReportManagement;
using ReportManagement.Domain.Shared.ReportManagement.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ReportManagement.EntityFrameworkCore.ReportManagement.EntityFrameworkCore.Repositories
{
    public class WidgetRepository :EfCoreRepository<ReportManagementDbContext, Widget, int>, IWidgetRepository
    {
        private readonly IConfiguration _configuration;
        private string _connectionStrings;
        public WidgetRepository(IConfiguration configuration, IDbContextProvider<ReportManagementDbContext> dbContextProvider)
          : base(dbContextProvider)
        {
            _configuration = configuration;
            _connectionStrings = _configuration.GetConnectionString("ReportManagement");
        }

        public IEnumerable<dynamic> GetReportData(string command)
        {
            using (var connection = new SqlConnection(_connectionStrings))
            {
                var _ret = connection.Query(command,commandType: CommandType.Text).ToList();
                return _ret;
            }
        }

    }
}

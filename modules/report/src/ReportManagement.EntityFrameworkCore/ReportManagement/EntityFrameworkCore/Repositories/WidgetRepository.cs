using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Nest;
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
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ReportManagement.EntityFrameworkCore.ReportManagement.EntityFrameworkCore.Repositories
{
    public class WidgetRepository :EfCoreRepository<ReportManagementDbContext, Widget, int>, IWidgetRepository
    {
        private readonly IConfiguration _configuration;
        public WidgetRepository(IConfiguration configuration, IDbContextProvider<ReportManagementDbContext> dbContextProvider)
          : base(dbContextProvider)
        {
            _configuration = configuration;
        }

        public List<Dictionary<string, object>> GetReportData(string command)
        {
            var values = new List<Dictionary<string, object>>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ReportManagement")))
            {
                SqlCommand cmd = new SqlCommand(command, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var fieldValues = new Dictionary<string, object>();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        fieldValues.Add(rdr.GetName(i),rdr[i]);
                    }
                    values.Add(fieldValues);
                }
                con.Close();
                rdr.Close();

            }
            return values;
        }



    }
}

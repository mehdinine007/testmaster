using ReportManagement.Domain.ReportManagement;
using ReportManagement.Domain.Shared.ReportManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReportManagement.EntityFrameworkCore.ReportManagement.EntityFrameworkCore.Repositories
{
    public interface IWidgetRepository: IRepository<Widget, int>
    {
        IEnumerable<dynamic> GetReportData(string command);

    }
}

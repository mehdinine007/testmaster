using Microsoft.EntityFrameworkCore;
using ReportManagement.Domain.ReportManagement;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;


namespace ReportManagement.EntityFrameworkCore
{
    [ConnectionStringName("ReportManagement")]
    public interface IReportManagementDbContext : IEfCoreDbContext
    {
        DbSet<Dashboard> Dashboards { get; set; }
        DbSet<Widget> Widgets { get; set; }
        DbSet<DashboardWidget> DashboardWidgets { get; set; }
     
    }
}
using AdminPanelManagement.Domain.AdminPanelManagement;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;


namespace AdminPanelManagement.EntityFrameworkCore
{
    [ConnectionStringName("AdminPanelManagement")]
    public interface IAdminPanelManagementDbContext : IEfCoreDbContext
    {
        // DbSet<OrganizationChart> OrganizationCharts { get; set; }
        DbSet<Test> Test { get; set; }

    }
}
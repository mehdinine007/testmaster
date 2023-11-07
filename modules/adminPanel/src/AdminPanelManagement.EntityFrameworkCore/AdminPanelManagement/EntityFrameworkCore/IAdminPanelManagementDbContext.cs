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
        DbSet<Test> Test { get; set; }
    
    }
}
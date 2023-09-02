using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.EntityFrameworkCore
{
    [ConnectionStringName("WorkflowManagement")]
    public interface IWorkFlowManagementDbContext : IEfCoreDbContext
    {
        DbSet<OrganizationChart> OrganizationCharts { get; set; }
        DbSet<OrganizationPosition> OrganizationPositions { get; set; }
        DbSet<WorkFlowRole> Roles { get; set; }
        DbSet<WorkFlowRoleChart> RoleCharts { get; set; }
    }
}
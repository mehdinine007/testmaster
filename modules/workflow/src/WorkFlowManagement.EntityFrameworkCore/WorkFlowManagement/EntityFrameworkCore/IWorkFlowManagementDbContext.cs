using Microsoft.EntityFrameworkCore;
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
        DbSet<WorkFlowRole> WorkFlowRoles { get; set; }
    }
}
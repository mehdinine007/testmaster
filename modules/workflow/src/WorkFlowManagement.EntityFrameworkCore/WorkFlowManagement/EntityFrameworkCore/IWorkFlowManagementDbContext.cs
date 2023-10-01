using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
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
        DbSet<Role> Roles { get; set; }
        DbSet<RoleOrganizationChart> RoleOrganizationCharts { get; set; }
        DbSet<Scheme> Schemes { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<Transition> Transitions { get; set; }
        DbSet<ActivityRole> ActivityRoles { get; set; }
        DbSet<Process> Processes { get; set; }
        DbSet<Inbox> Inbox { get; set; }
        DbSet<Person> Person { get; set; }
    }
}
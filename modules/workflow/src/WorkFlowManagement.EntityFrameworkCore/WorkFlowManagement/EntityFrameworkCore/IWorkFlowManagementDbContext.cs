using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.EntityFrameworkCore
{
    [ConnectionStringName("WorkflowManagement")]   
    public interface IWorkFlowManagementDbContext : IEfCoreDbContext
    {
        DbSet<OrganizationChart> OrganizationChart { get; set; }
    }
}
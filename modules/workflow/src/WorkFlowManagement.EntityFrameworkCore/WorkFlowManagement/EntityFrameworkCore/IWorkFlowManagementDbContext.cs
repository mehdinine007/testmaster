using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace WorkFlowManagement.EntityFrameworkCore
{
    [ConnectionStringName("WorkFlowManagement")]
    public interface IWorkFlowManagementDbContext : IEfCoreDbContext
    {
    }
}
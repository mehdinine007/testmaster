using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace GatewayManagement.EntityFrameworkCore
{
    [ConnectionStringName("GatewayManagement")]
    public interface IGatewayManagementDbContext : IEfCoreDbContext
    {
    }
}
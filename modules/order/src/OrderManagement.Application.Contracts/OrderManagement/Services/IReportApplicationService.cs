using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IReportApplicationService : IApplicationService
    {
        Task<object> Test();
    }
}

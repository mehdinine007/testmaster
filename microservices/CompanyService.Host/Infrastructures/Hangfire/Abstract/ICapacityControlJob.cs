using System.Threading.Tasks;

namespace OrderService.Host.Infrastructures.Hangfire.Abstract
{
    public interface ICapacityControlJob
    {
        Task SaleDetail();
        Task Payment();
    }
}

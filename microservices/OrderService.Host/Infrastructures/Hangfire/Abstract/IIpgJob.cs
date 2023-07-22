using System.Threading.Tasks;

namespace OrderService.Host.Infrastructures.Hangfire.Abstract
{
    public interface IIpgJob
    {
         Task RetryForVerify();
        Task RetryOrderForVerify();
    }
}

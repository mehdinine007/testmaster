using System.Threading.Tasks;

namespace OrderService.Host.Infrastructures.Hangfire.Abstract
{
    public interface ISignJob
    {
         Task CheckDigitalSign();
    }
}

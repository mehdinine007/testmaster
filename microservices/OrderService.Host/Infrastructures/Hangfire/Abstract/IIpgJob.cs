namespace OrderService.Host.Infrastructures.Hangfire.Abstract
{
    public interface IIpgJob
    {
        void RetryForVerify();
    }
}

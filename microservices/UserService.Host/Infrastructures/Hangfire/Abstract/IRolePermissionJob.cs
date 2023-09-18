namespace UserService.Host.Infrastructures.Hangfire.Abstract
{
    public interface IRolePermissionJob
    {
        Task AddToRedis();
    }
}

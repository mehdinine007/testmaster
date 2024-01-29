using Hangfire.Dashboard;

namespace BankService.Host.Infrastructures.Hangfire
{
    public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
        {
            return true;
        }
    }
}

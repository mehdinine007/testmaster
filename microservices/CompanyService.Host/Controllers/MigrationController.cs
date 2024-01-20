using IFG.Core.Utility.Migration;
using Licence;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace CompanyManagement.Controllers
{
    [RemoteService]
    [Route("api/services/app/[controller]/[action]")]
    public class MigrationController : Controller
    {
        private readonly MigrationsHistoryService _migrationsHistoryService;
        public MigrationController()
        {
            _migrationsHistoryService = new MigrationsHistoryService("CompanyManagement", AppLicence.GetVersion("").FixVersion);
        }

        [HttpPost]
        public bool UpdateDataBase()
        {
            return _migrationsHistoryService.UpdateDatabase();
        }

    }
}

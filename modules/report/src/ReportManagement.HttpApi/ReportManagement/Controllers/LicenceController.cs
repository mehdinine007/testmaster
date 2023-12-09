using Licence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace ReportManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/[controller]/[action]")]
    public class LicenceController : Controller
    {
        private readonly IConfiguration _configuration;
        public LicenceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public VersionInfo GetInfo()
        {
            return AppLicence.GetVersion(_configuration.GetSection("Licence:SerialNumber").Value);
        }
    }
}

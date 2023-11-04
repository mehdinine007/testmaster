using Licence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace PaymentManagement.HttpApi.UserManagement.Controllers
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

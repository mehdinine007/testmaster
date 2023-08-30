#region NS
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
#endregion


namespace UserService.Host.Controllers
{
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}

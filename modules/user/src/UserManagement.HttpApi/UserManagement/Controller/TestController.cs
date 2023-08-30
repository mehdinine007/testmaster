#region NS
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.UserManagement.Implementations;
using Volo.Abp.AspNetCore.Mvc;
# endregion


namespace UserManagement.HttpApi.UserManagement.Controller
{
    [Route("api/Test")]
    public class TestController : AbpController
    {
        [HttpGet("TestData")]
        public IActionResult GetTestData()
        {
            var testData = TestService.GetTestData();
            return new ObjectResult(testData);
        }
    }
}

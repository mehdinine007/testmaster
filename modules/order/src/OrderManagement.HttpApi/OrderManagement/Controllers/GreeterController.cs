#region NS
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OrderManagement.Application.OrderManagement.Implementations;
#endregion

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[ApiController]
[Route("api/RpcTest/[controller]")]
public class GreeterController : ControllerBase
{
    private readonly GreeterClientGrpcService _greeterClient;

    public GreeterController(GreeterClientGrpcService greeterClient)
    {
        _greeterClient = greeterClient;
    }

    #region Method
    [HttpGet]
    public async Task<IActionResult> GetId()
    {
        var result = await _greeterClient.GetId();

        if (result is OkObjectResult okResult)
        {
            var message = okResult.Value.ToString();
            return Ok(message);
        }
        else
        {
            return BadRequest();
        }
    }
    #endregion

}

using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace OrderManagement.HttpApi;

//[Route("api/services/app/psp/[action]")]
public class PspController : AbpController
{
    private readonly IOrderAppService _orderAppService;
    private readonly IConfiguration _configuration;

    public PspController(IOrderAppService orderAppService, IConfiguration configuration)
    {
        _orderAppService = orderAppService;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> CallBack(string data)
    {
        var callBackRequest = JsonConvert.DeserializeObject<IPgCallBackRequest>(data);
        var paymentResult = await _orderAppService.CheckoutPayment(callBackRequest);
        //TODO: complete front end call back url
        return Redirect(string.Format(_configuration.GetValue<string>("ClientSideCallbackUrl"),paymentResult.Status,paymentResult.OrderId));
    }
}

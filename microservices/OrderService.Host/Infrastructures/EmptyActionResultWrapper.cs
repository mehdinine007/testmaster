using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace OrderService.Host.Infrastructures;

public class EmptyActionResultWrapper : IActionResultWrapper
{
    public void Wrap(FilterContext context)
    {
        switch (context)
        {
            case ResultExecutingContext resultExecutingContext:
                resultExecutingContext.Result = new ObjectResult(ApiResult.InitilizeSuccessfullApiResult(new string[0]));
                return;

            case PageHandlerExecutedContext pageHandlerExecutedContext:
                pageHandlerExecutedContext.Result = new ObjectResult(ApiResult.InitilizeSuccessfullApiResult(new string[0]));
                return;
        }
    }
}

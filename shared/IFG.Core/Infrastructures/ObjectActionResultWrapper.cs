using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IFG.Core.Infrastructures;

public class ObjectActionResultWrapper : IActionResultWrapper
{
    public void Wrap(FilterContext context)
    {
        ObjectResult objectResult = null;

        switch (context)
        {
            case ResultExecutingContext resultExecutingContext:
                objectResult = resultExecutingContext.Result as ObjectResult;
                break;

            case PageHandlerExecutedContext pageHandlerExecutedContext:
                objectResult = pageHandlerExecutedContext.Result as ObjectResult;
                break;
        }

        if (objectResult == null)
        {
            throw new ArgumentException("Action Result should be JsonResult!");
        }

        if (!(objectResult.Value is ApiResult))
        {
            objectResult.Value = ApiResult.InitilizeSuccessfullApiResult(objectResult.Value);
            objectResult.DeclaredType = typeof(ApiResult);
        }
    }
}
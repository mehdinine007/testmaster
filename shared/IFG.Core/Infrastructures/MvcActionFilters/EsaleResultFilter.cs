using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IFG.Core.Infrastructures.MvcActionFilters;

public class EsaleResultFilter : IResultFilter
{
    private readonly IActionResultWrapperFactory _actionResultWrapperFactory;

    public EsaleResultFilter(IActionResultWrapperFactory actionResultWrapperFactory)
    {
        _actionResultWrapperFactory = actionResultWrapperFactory;
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (!(context.ActionDescriptor is ControllerActionDescriptor))
        {
            return;
        }
        _actionResultWrapperFactory.CreateFor(context).Wrap(context);
    }
}

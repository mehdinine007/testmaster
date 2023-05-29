using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PaymentService.Host.Infrastructures;

public class EsaleResultFilter : IResultFilter
{
    private readonly IActionResultWrapperFactory _actionResultWrapperFactory;

    public EsaleResultFilter(IActionResultWrapperFactory actionResultWrapperFactory)
    {
        _actionResultWrapperFactory = actionResultWrapperFactory;
    }


    public virtual void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            return;
        }
        _actionResultWrapperFactory.CreateFor(context).Wrap(context);
    }
}

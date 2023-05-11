using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace OrderService.Host.Infrastructures;

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

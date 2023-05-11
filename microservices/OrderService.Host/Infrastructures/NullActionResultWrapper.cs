using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderService.Host.Infrastructures;

public class NullActionResultWrapper : IActionResultWrapper
{
    public void Wrap(FilterContext context)
    {

    }

}
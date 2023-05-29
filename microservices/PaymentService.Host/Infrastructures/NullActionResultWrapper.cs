using Microsoft.AspNetCore.Mvc.Filters;

namespace PaymentService.Host.Infrastructures;

public class NullActionResultWrapper : IActionResultWrapper
{
    public void Wrap(FilterContext context)
    {

    }

}
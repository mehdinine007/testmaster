using Microsoft.AspNetCore.Mvc.Filters;

namespace PaymentService.Host.Infrastructures;

public interface IActionResultWrapper
{
    void Wrap(FilterContext context);
}

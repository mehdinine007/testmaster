using Microsoft.AspNetCore.Mvc.Filters;

namespace Esale.Core.Infrastructures;

public interface IActionResultWrapper
{
    void Wrap(FilterContext context);
}

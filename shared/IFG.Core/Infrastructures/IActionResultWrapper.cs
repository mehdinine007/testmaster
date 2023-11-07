using Microsoft.AspNetCore.Mvc.Filters;

namespace IFG.Core.Infrastructures;

public interface IActionResultWrapper
{
    void Wrap(FilterContext context);
}

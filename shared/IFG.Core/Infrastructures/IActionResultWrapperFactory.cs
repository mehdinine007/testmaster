using Microsoft.AspNetCore.Mvc.Filters;

namespace IFG.Core.Infrastructures;

public interface  IActionResultWrapperFactory 
{
    IActionResultWrapper CreateFor(FilterContext context);
}

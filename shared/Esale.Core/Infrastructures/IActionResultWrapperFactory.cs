using Microsoft.AspNetCore.Mvc.Filters;

namespace Esale.Core.Infrastructures;

public interface  IActionResultWrapperFactory 
{
    IActionResultWrapper CreateFor(FilterContext context);
}

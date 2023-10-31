using Microsoft.AspNetCore.Mvc.Filters;

namespace IFG.Core.Infrastructures;

public class NullActionResultWrapper : IActionResultWrapper
{
    public void Wrap(FilterContext context)
    {

    }

}
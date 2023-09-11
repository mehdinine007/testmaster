using Microsoft.AspNetCore.Mvc.Filters;

namespace Esale.Core.Infrastructures;

public class NullActionResultWrapper : IActionResultWrapper
{
    public void Wrap(FilterContext context)
    {

    }

}
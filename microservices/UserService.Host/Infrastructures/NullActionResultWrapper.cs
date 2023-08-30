#region NS
using Microsoft.AspNetCore.Mvc.Filters;
#endregion


namespace UserService.Host.Infrastructures;

public class NullActionResultWrapper : IActionResultWrapper
{
    public void Wrap(FilterContext context)
    {

    }

}
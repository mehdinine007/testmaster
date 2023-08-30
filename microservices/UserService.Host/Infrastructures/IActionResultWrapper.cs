#region NS
using Microsoft.AspNetCore.Mvc.Filters;
#endregion

namespace UserService.Host.Infrastructures;

public interface IActionResultWrapper
{
    void Wrap(FilterContext context);
}

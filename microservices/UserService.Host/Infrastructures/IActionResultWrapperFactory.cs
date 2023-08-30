#region NS
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.DependencyInjection;
#endregion


namespace UserService.Host.Infrastructures;

public interface IActionResultWrapperFactory : ITransientDependency
{
    IActionResultWrapper CreateFor(FilterContext context);
}

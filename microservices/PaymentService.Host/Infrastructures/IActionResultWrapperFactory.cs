using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.DependencyInjection;

namespace PaymentService.Host.Infrastructures;

public interface IActionResultWrapperFactory : ITransientDependency
{
    IActionResultWrapper CreateFor(FilterContext context);
}

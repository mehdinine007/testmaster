using IFG.Core.Infrastructures;
using IFG.Core.Infrastructures.MvcActionFilters;
using Microsoft.Extensions.DependencyInjection;

namespace IFG.Core.Extensions;

public static class EsaleCoreMvcExtension
{
    public static void AddEsaleResultWrapper(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider();
        services.AddTransient<IActionResultWrapperFactory, EsaleActionResultWrapperFactory>();
        services.AddControllers(opt => opt.Filters.Add<EsaleResultFilter>());
    }
}

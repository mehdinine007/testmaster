using Esale.Core.Infrastructures;
using Esale.Core.Infrastructures.MvcActionFilters;
using Microsoft.Extensions.DependencyInjection;

namespace Esale.Core.Extensions;

public static class EsaleCoreMvcExtension
{
    public static void AddEsaleResultWrapper(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider();
        services.AddTransient<IActionResultWrapperFactory, EsaleActionResultWrapperFactory>();
        services.AddControllers(opt => opt.Filters.Add<EsaleResultFilter>());
    }
}

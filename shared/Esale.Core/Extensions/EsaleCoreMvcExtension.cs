using Esale.Core.Infrastructures.MvcActionFilters;
using Microsoft.Extensions.DependencyInjection;

namespace Esale.Core.Extensions;

public static class EsaleCoreMvcExtension
{
    public static void AddEsaleResultFilter(this IServiceCollection services)
    {
        services.AddControllers(opt => opt.Filters.Add<EsaleResultFilter>());
    }
}

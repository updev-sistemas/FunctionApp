using FunctionApp.Infrastructure.Profiles;
using FunctionApp.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionApp.Infrastructure.Extension;

public static class FunctionAppExtension
{
    public static void RegisterRepository(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IPersonRepository, PersonRepository>();
    }

    public static void Automap(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddAutoMapper(typeof(PersonProfile));
    }
}

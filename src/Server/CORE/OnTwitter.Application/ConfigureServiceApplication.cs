using System.Reflection;
using Microsoft.Extensions.DependencyInjection;


namespace OnTwitter.Application;

public static class ConfigureServiceApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}

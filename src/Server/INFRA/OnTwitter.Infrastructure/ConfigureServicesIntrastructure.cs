using OnTwitter.Application.Common.Interfaces;
using OnTwitter.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnTwitter.Infrastructure.Services;

namespace OnTwitter.Infrastructure;

public static class ConfigureServicesIntrastructure
{
 
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Here we can set any Database Provider 
        services.AddDbContext<TwitterDbContext>(options => options.UseInMemoryDatabase("TwitterDb"));
        services.AddScoped<ITwitterDbContext, TwitterDbContext>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<ITwitterService, TwitterService>();


        return services;
    }
}

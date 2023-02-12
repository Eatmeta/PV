using Api.Data;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ExamplesApi"];
        services.AddDbContext<ExamplesDbContext>(options => { options.UseNpgsql(connectionString); });
        services.AddScoped<IExamplesDbContext>(provider => provider.GetService<ExamplesDbContext>());
        return services;
    }
}
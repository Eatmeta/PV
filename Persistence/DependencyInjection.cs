using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<ExamplesDbContext>((serviceProvider, dbContextOptionsBuilder) =>
        {
            dbContextOptionsBuilder.UseNpgsql(
                serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("ExamplesApi"),
                npgsqlDbContextOptionsBuilder =>
                    npgsqlDbContextOptionsBuilder.MigrationsAssembly("Persistence"));
        });
        
        services.AddScoped<IExamplesDbContext>(provider => provider.GetService<ExamplesDbContext>());
        
        return services;
    }
}
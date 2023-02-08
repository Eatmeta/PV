﻿using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        services.AddDbContext<ExamplesDbContext>(options => { options.UseSqlite(connectionString); });
        services.AddScoped<IExamplesDbContext>(provider => provider.GetService<ExamplesDbContext>());
        return services;
    }
}
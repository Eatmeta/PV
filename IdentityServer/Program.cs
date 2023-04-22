using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityServer.Data;
using IdentityServer.Factories;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    dbContextOptionsBuilder.UseNpgsql(
        serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("Identity"),
        NpgsqlOptionsAction);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(identityOptions =>
    {
        identityOptions.User.RequireUniqueEmail = true;
    })
    .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(configurationStoreOptions =>
    {
        configurationStoreOptions.ResolveDbContextOptions = ResolveDbContextOptions;
    })
    .AddOperationalStore(operationalStoreOptions =>
    {
        operationalStoreOptions.ResolveDbContextOptions = ResolveDbContextOptions;
    });

builder.Services.AddRazorPages();

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (await userManager.FindByNameAsync("eat.meta") == null)
    {
        await userManager.CreateAsync(
            new ApplicationUser
            {
                UserName = "eat.meta",
                Email = "eat.meta@example.com",
                GivenName = "Eat",
                FamilyName = "Meta"
            }, "Pa55w0rd!");
    }

    var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

    if (!await configurationDbContext.ApiResources.AnyAsync())
    {
        await configurationDbContext.ApiResources.AddAsync(new ApiResource
        {
            Name = Guid.Parse("946bdf76-f6c4-4404-babd-c75414cbbed7").ToString(),
            DisplayName = "API",
            Scopes = new List<string> {"https://www.example.com/api"}
        }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.ApiScopes.AnyAsync())
    {
        await configurationDbContext.ApiScopes.AddAsync(new ApiScope
        {
            Name = "https://www.example.com/api",
            DisplayName = "API"
        }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.Clients.AnyAsync())
    {
        await configurationDbContext.Clients.AddRangeAsync(
            new Client
            {
                ClientId = Guid.Parse("e96e57fe-8ecb-4d9a-8168-e286d0d5df8b").ToString(),
                ClientSecrets = new List<Secret> {new("secret".Sha512())},
                ClientName = "Console Application",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string> {"https://www.example.com/api"},
                AllowedCorsOrigins = new List<string> {"https://api:7001"}
            }.ToEntity(),
            new Client
            {
                ClientId = Guid.Parse("1ecb3a93-3695-4652-a83a-5a536ef3f4fd").ToString(),
                ClientSecrets = new List<Secret> {new("secret".Sha512())},
                ClientName = "Web Application",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string> {"openid", "profile", "email", "https://www.example.com/api"},
                RedirectUris = new List<string> {"https://blazorserver:7002/signin-oidc"},
                PostLogoutRedirectUris = new List<string> {"https://blazorserver:7002/signout-callback-oidc"}
            }.ToEntity(),
            new Client
            {
                ClientId = Guid.Parse("03037fa7-c389-4ece-8efc-ca23298028db").ToString(),
                RequireClientSecret = false,
                ClientName = "Single Page Application",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string> {"openid", "profile", "email", "https://www.example.com/api"},
                AllowedCorsOrigins = new List<string> {"http://singlepageapplication:7003"},
                RedirectUris =
                    new List<string> {"http://singlepageapplication:7003/authentication/login-callback"},
                PostLogoutRedirectUris =
                    new List<string> {"http://singlepageapplication:7003/authentication/logout-callback"}
            }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.IdentityResources.AnyAsync())
    {
        await configurationDbContext.IdentityResources.AddRangeAsync(
            new IdentityResources.OpenId().ToEntity(),
            new IdentityResources.Profile().ToEntity(),
            new IdentityResources.Email().ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }
}

app.Run();

void NpgsqlOptionsAction(NpgsqlDbContextOptionsBuilder npgsqlDbContextOptionsBuilder)
{
    npgsqlDbContextOptionsBuilder.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
}

void ResolveDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder dbContextOptionsBuilder)
{
    dbContextOptionsBuilder.UseNpgsql(
        serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("IdentityServer"),
        NpgsqlOptionsAction);
}
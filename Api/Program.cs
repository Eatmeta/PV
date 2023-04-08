using System.Reflection;
using Api;
using Api.Middleware;
using Application;
using Application.Common.Mappings;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IExamplesDbContext).Assembly));
});

builder.Services.AddApplication();

builder.Services.AddPersistence();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtBearerOptions =>
    {
        jwtBearerOptions.Authority = builder.Configuration["Authentication:Authority"];
        jwtBearerOptions.Audience = builder.Configuration["Authentication:Audience"];

        jwtBearerOptions.TokenValidationParameters.ValidateAudience = true;
        jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
        jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
    });

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy("ApiScope",
        authorizationPolicyBuilder =>
        {
            authorizationPolicyBuilder.RequireAuthenticatedUser()
                .RequireClaim("scope", "https://www.example.com/api");
        });
});

builder.Services.AddControllers();

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    swaggerGenOptions.IncludeXmlComments(xmlPath);
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>>(x =>
    new ConfigureSwaggerOptions($"{builder.Configuration["Authentication:Authority"]}"));

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

/*using var scope = builder.Services.BuildServiceProvider().CreateScope();

var serviceProvider = scope.ServiceProvider;
try
{
    var context = serviceProvider.GetRequiredService<ExamplesDbContext>();
    DbInitializer.Initialize(context);
}
catch (Exception exception)
{
    // ignored
}*/

app.Run();

/*void NpgsqlOptionsAction(NpgsqlDbContextOptionsBuilder npgsqlDbContextOptionsBuilder)
{
    npgsqlDbContextOptionsBuilder.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
}*/
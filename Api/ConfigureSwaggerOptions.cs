using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly string _authorityString;

    public ConfigureSwaggerOptions(string authorityString)
    {
        _authorityString = authorityString;
    }
    
    public void Configure(SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = $"PhrasalVerb API",
                Description = "PhrasalVerb Api for PhrasalVerb Game",
                Contact = new OpenApiContact
                {
                    Name = "Eatmeta",
                    Email = string.Empty,
                    Url = new Uri("https://www.linkedin.com/in/eatmeta")
                }
            });

        swaggerGenOptions.AddSecurityDefinition("oauth2",
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(_authorityString + "/connect/token"),
                        Scopes = {{"https://www.example.com/api", "API"}}
                    }
                }
            });

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                },
                new List<string> {"https://www.example.com/api"}
            }
        });
    
        swaggerGenOptions.CustomOperationIds(apiDescription =>
            apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                ? methodInfo.Name
                : null);
    }
}
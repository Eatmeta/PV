using BlazorServerApp;
using BlazorServerApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false);

builder.Services.AddScoped<IExampleDetailsService, ExampleDetailsService>();

builder.Services.AddAuthentication(authenticationOptions =>
    {
        authenticationOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        authenticationOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(openIdConnectOptions =>
    {
        openIdConnectOptions.Authority = builder.Configuration["Authentication:Authority"];
        openIdConnectOptions.ClientId = builder.Configuration["Authentication:ClientId"];
        openIdConnectOptions.ClientSecret = builder.Configuration["Authentication:ClientSecret"];
        openIdConnectOptions.GetClaimsFromUserInfoEndpoint = true;
        openIdConnectOptions.ResponseType = "code";
        openIdConnectOptions.Scope.Add("https://www.example.com/api");
        openIdConnectOptions.SaveTokens = true;
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<SessionProperties>();

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
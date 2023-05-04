using BlazorServerApp;
using BlazorServerApp.Services;
using BlazorServerApp.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings2.json", optional: true, reloadOnChange: false)
    .AddJsonFile("appsettings2.Development.json", optional: true, reloadOnChange: false);

Sd.ExampleApiBase = builder.Configuration["ServiceUrls:ExampleAPI"];

builder.Services.AddHttpClient<IExampleDetailsService, ExampleDetailsService>();
builder.Services.AddHttpClient<IExampleAttemptService, ExampleAttemptService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IExampleDetailsService, ExampleDetailsService>();
builder.Services.AddScoped<IExampleAttemptService, ExampleAttemptService>();

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
builder.Services.AddScoped<TokenProvider>();

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
using System.Net.Http.Headers;
using Application.Examples.Queries.GetExampleDetails;
using Application.Examples.Queries.GetExampleList;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BlazorServerApp.Services;

//[Authorize]
public class ExampleDetailsService : IExampleDetailsService
{
    private IHttpClientFactory HttpClientFactory { get; }
    private IHttpContextAccessor HttpContextAccessor { get; }
    
    public ExampleDetailsService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        HttpClientFactory = httpClientFactory;
        HttpContextAccessor = httpContextAccessor;
    }

    public async Task<ExampleDetailsDto?> GetRandomExampleDetails()
    {
        using var httpClient = HttpClientFactory.CreateClient();

        try
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await HttpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            
            var response = await httpClient.GetAsync("https://api:7001/api/Example/GetRandomExampleDetails");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<ExampleDetailsDto>();
            }
            else 
            { 
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} message: {message}");
            }
        }
        catch (Exception)
        {
            //Log exception
            throw;
        }
    }
    
    public async Task<ExampleListDto?> GetAllExamples()
    {
        using var httpClient = HttpClientFactory.CreateClient();

        try
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await HttpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            
            var response = await httpClient.GetAsync("https://api:7001/api/Example/GetListOfExamples");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<ExampleListDto>();
            }
            else 
            { 
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} message: {message}");
            }
        }
        catch (Exception)
        {
            //Log exception
            throw;
        }
    }
}
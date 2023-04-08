using System.Net.Http.Headers;
using System.Text.Json;
using Application.Examples.Queries.GetExampleDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BlazorServerApp.Pages;

[Authorize]
public class ApiModel : PageModel
{
    public ExampleDetailsDto? Example { get; set; } = new ();
    private IHttpClientFactory HttpClientFactory { get; }
    
    public ApiModel(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        using var httpClient = HttpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));

        var dto = await httpClient.GetStringAsync("https://api:7001/api/Example/GetRandomExampleDetails");
        
        Example = JsonConvert.DeserializeObject<ExampleDetailsDto>(dto);

    }
}
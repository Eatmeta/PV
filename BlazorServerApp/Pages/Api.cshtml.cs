using System.Net.Http.Headers;
using System.Text.Json;
using BlazorServerApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhrasalVerb.Domain;

namespace BlazorServerApp.Pages;

//[Authorize]
public class ApiModel : PageModel
{
    public ApiModel(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
    }

    public Example? Example { get; set; }

    private IHttpClientFactory HttpClientFactory { get; }

    public async Task OnGetAsync()
    {
        using var httpClient = HttpClientFactory.CreateClient();

        /*httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));

        var fromDb = await httpClient.GetStringAsync("https://api:7001/api/Example/GetRandomExampleDetails");

        Example = JsonSerializer.Deserialize<Example>(fromDb);*/
        
        Example = new Example
        {
            ExampleFull = "We have to ABIDE BY what the court says.",
            ExampleFullUnderscore = "We have to _____ __ what the court says.",
            ExampleId = Guid.NewGuid(),
            ExampleParticle = "BY",
            ExampleVerb = "ABIDE",
            Meaning = "Accept or follow a decision or rule.",
            Verb = "Abide",
            VerbAndParticle = "Abide by"
        }; 
    }
}
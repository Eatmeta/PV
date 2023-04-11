using Application.Examples.Queries.GetExampleDetails;

namespace BlazorServerApp.Services;

public interface IExampleDetailsService
{
    Task<ExampleDetailsDto?> GetRandomExampleDetails();
}
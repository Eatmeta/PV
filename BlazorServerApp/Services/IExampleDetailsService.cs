using Application.Examples.Queries.GetExampleDetails;
using Application.Examples.Queries.GetExampleList;

namespace BlazorServerApp.Services;

public interface IExampleDetailsService
{
    Task<ExampleDetailsDto?> GetRandomExampleDetails();
    Task<ExampleListDto> GetAllExamples();
}
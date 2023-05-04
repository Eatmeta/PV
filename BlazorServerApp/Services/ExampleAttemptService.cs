using BlazorServerApp.Models;
using BlazorServerApp.Services.IServices;

namespace BlazorServerApp.Services;

public class ExampleAttemptService : BaseService, IExampleAttemptService
{
    private IHttpClientFactory HttpClientFactory { get; }
    private readonly TokenProvider _tokenProvider;

    public ExampleAttemptService(IHttpClientFactory httpClientFactory, TokenProvider tokenProvider)
        : base(httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
    }

    public async Task<T> CreateExampleAttemptAsync<T>(CreateExampleAttemptDto dto)
    {
        var token = _tokenProvider.AccessToken;
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = Sd.ApiType.Post,
            Data = dto,
            Url = Sd.ExampleApiBase + "/api/ExampleAttempt/CreateExampleAttempt",
            AccessToken = token
        });
    }
}
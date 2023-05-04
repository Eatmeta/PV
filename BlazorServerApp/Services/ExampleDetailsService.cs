using System.Net.Http.Headers;
using Application.Examples.Queries.GetExampleDetails;
using Application.Examples.Queries.GetExampleList;
using BlazorServerApp.Models;
using BlazorServerApp.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BlazorServerApp.Services;

public class ExampleDetailsService : BaseService, IExampleDetailsService
{
    private IHttpClientFactory HttpClientFactory { get; }
    private readonly TokenProvider _tokenProvider;

    public ExampleDetailsService(IHttpClientFactory httpClientFactory, TokenProvider tokenProvider)
        : base(httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
    }

    public async Task<T> GetRandomExampleDetails<T>()
    {
        var token = _tokenProvider.AccessToken;
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = Sd.ApiType.Get,
            Url = Sd.ExampleApiBase + "/api/Example/GetRandomExampleDetails",
            AccessToken = token
        });
    }
}
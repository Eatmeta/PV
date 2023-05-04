using System.Net.Http.Headers;
using System.Text;
using BlazorServerApp.Models;
using BlazorServerApp.Services.IServices;
using Newtonsoft.Json;

namespace BlazorServerApp.Services;

public class BaseService : IBaseService
{
    private IHttpClientFactory HttpClientFactory { get; }
    public ResponseDto ResponseModel { get; set; }

    public BaseService(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
        ResponseModel = new ResponseDto();
    }

    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = HttpClientFactory.CreateClient();
            var message = new HttpRequestMessage();
            
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            client.DefaultRequestHeaders.Clear();
            
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrEmpty(apiRequest.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
            }

            HttpResponseMessage apiResponse = null;
            message.Method = apiRequest.ApiType switch
            {
                Sd.ApiType.Post => HttpMethod.Post,
                Sd.ApiType.Put => HttpMethod.Put,
                Sd.ApiType.Delete => HttpMethod.Delete,
                _ => HttpMethod.Get
            };
            apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            Console.WriteLine(apiContent);
            var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
            return apiResponseDto;
        }
        catch (Exception e)
        {
            var dto = new ResponseDto
            {
                DisplayMessage = "Error",
                ErrorMessages = new List<string> {Convert.ToString(e.Message)},
                IsSuccess = false
            };
            var res = JsonConvert.SerializeObject(dto);
            var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
            return apiResponseDto;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }
}
using BlazorServerApp.Models;

namespace BlazorServerApp.Services.IServices;

public interface IBaseService : IDisposable
{
    ResponseDto ResponseModel { get; set; }
    Task<T> SendAsync<T>(ApiRequest apiRequest);
}
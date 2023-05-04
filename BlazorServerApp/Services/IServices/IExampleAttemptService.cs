using BlazorServerApp.Models;

namespace BlazorServerApp.Services.IServices;

public interface IExampleAttemptService
{
    Task<T> CreateExampleAttemptAsync<T>(CreateExampleAttemptDto dto);
}
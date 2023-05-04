namespace BlazorServerApp.Services.IServices;

public interface IExampleDetailsService
{
    Task<T> GetRandomExampleDetails<T>();
}
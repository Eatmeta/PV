namespace BlazorServerApp;

public static class Sd
{
    public static string ExampleApiBase { get; set; }
    
    public enum ApiType
    {
        Get,
        Post,
        Put,
        Delete
    }
}
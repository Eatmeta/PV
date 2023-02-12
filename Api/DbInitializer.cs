using Api.Data;

namespace Api;

public class DbInitializer
{
    public static void Initialize(ExamplesDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
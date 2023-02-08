namespace Persistence;

public class DbInitializer
{
    public static void Initialize(ExamplesDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
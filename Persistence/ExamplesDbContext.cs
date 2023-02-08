using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityTypeConfigurations;
using PhrasalVerb.Domain;

namespace Persistence;

public class ExamplesDbContext : DbContext, IExamplesDbContext
{
    public DbSet<Example> Examples { get; set; }

    public ExamplesDbContext(DbContextOptions<ExamplesDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder) 
    {
        builder.ApplyConfiguration(new ExampleConfiguration());
        base.OnModelCreating(builder);
    }
}
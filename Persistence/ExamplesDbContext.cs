using System.Reflection;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ExamplesDbContext : DbContext, IExamplesDbContext
{
    public DbSet<Example> Examples { get; set; }
    public DbSet<Meaning> Meanings { get; set; }
    public DbSet<Verb> Verbs { get; set; }
    public DbSet<PhrasalVerb> PhrasalVerbs { get; set; }
    public DbSet<Particle> Particles { get; set; }
    public DbSet<ExampleAttempt> ExampleAttempts { get; set; }

    public ExamplesDbContext(DbContextOptions<ExamplesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
        //new DbInitializer(builder).Seed();
    }
}
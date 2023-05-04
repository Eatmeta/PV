using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IExamplesDbContext
{
    public DbSet<Example> Examples { get; set; }
    public DbSet<Meaning> Meanings { get; set; }
    public DbSet<Verb> Verbs { get; set; }
    public DbSet<PhrasalVerb> PhrasalVerbs { get; set; }
    public DbSet<Particle> Particles { get; set; }
    public DbSet<ExampleAttempt> ExampleAttempts { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
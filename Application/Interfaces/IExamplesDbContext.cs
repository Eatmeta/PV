using Microsoft.EntityFrameworkCore;
using PhrasalVerb.Domain;

namespace Application.Interfaces;

public interface IExamplesDbContext
{
    DbSet<Example>? Examples { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
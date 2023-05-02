using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IExamplesDbContext
{
    DbSet<Example>? Examples { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
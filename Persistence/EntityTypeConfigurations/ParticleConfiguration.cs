using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class ParticleConfiguration : IEntityTypeConfiguration<Particle>
{
    public void Configure(EntityTypeBuilder<Particle> builder)
    {
        builder.HasKey(particle => particle.Id);
        
        builder.Property(particle => particle.Id).ValueGeneratedOnAdd();
        
        builder.Property(particle => particle.Body).IsRequired().HasMaxLength(30);
    }
}
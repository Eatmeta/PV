using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class ExampleConfiguration : IEntityTypeConfiguration<Example>
{
    public void Configure(EntityTypeBuilder<Example> builder)
    {
        builder.HasKey(example => example.Id);
        
        builder.Property(example => example.Id).ValueGeneratedOnAdd();
        
        builder.Property(example => example.Body).IsRequired().HasMaxLength(400);
        
        builder.HasOne(example => example.Verb)
            .WithMany(verb => verb.Examples)
            .HasForeignKey(example => example.VerbId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(example => example.Particle)
            .WithMany(particle => particle.Examples)
            .HasForeignKey(example => example.ParticleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(example => example.Meaning)
            .WithMany(meaning => meaning.Examples)
            .HasForeignKey(example => example.MeaningId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(example => example.PhrasalVerb)
            .WithMany(phrasalVerb => phrasalVerb.Examples)
            .HasForeignKey(example => example.PhrasalVerbId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}
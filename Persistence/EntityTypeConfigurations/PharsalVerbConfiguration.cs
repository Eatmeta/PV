using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class PharsalVerbConfiguration : IEntityTypeConfiguration<PhrasalVerb>
{
    public void Configure(EntityTypeBuilder<PhrasalVerb> builder)
    {
        builder.HasKey(phrasalVerb => phrasalVerb.Id);
        
        builder.Property(phrasalVerb => phrasalVerb.Id).ValueGeneratedOnAdd();
        
        builder.Property(phrasalVerb => phrasalVerb.Body).IsRequired().HasMaxLength(50);

        builder.HasOne(phrasalVerb => phrasalVerb.Particle)
            .WithMany(particle => particle.PhrasalVerbs)
            .HasForeignKey(phrasalVerb => phrasalVerb.ParticleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(phrasalVerb => phrasalVerb.Verb)
            .WithMany(verb => verb.PhrasalVerbs)
            .HasForeignKey(phrasalVerb => phrasalVerb.VerbId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
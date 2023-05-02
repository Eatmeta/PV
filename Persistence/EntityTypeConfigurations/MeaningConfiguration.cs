using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class MeaningConfiguration : IEntityTypeConfiguration<Meaning>
{
    public void Configure(EntityTypeBuilder<Meaning> builder)
    {
        builder.HasKey(meaning => meaning.Id);
        
        builder.Property(meaning => meaning.Id).ValueGeneratedOnAdd();
        
        builder.Property(meaning => meaning.Body).IsRequired().HasMaxLength(400);
    }
}
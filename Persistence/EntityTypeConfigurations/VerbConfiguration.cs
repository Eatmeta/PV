using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class VerbConfiguration : IEntityTypeConfiguration<Verb>
{
    public void Configure(EntityTypeBuilder<Verb> builder)
    {
        builder.HasKey(verb => verb.Id);
        
        builder.Property(verb => verb.Id).ValueGeneratedOnAdd();
        
        builder.Property(verb => verb.Body).IsRequired().HasMaxLength(30);
    }
}
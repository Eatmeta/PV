using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class ExampleAttemptConfiguration : IEntityTypeConfiguration<ExampleAttempt>
{
    public void Configure(EntityTypeBuilder<ExampleAttempt> builder)
    {
        builder.HasKey(exampleAttempt => exampleAttempt.Id);
        
        builder.Property(exampleAttempt => exampleAttempt.Id).ValueGeneratedOnAdd();
    }
}
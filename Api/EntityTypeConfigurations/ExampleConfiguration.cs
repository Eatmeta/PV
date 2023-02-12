using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhrasalVerb.Domain;

namespace Api.EntityTypeConfigurations;

public class ExampleConfiguration : IEntityTypeConfiguration<Example>
{
    public void Configure(EntityTypeBuilder<Example> builder)
    {
        builder.HasKey(example => example.ExampleId);
        builder.HasIndex(example => example.ExampleId).IsUnique();
    }
}
using Application.Common.Mappings;
using AutoMapper;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetExampleDetails;

public class ExampleDetailsDto : IMapWith<Example>
{
    public Guid ExampleId { get; set; }
    public string ExampleFull { get; set; } = string.Empty;
    public string ExampleFullUnderscore { get; set; } = string.Empty;
    public string ExampleVerb { get; set; } = string.Empty;
    public string ExampleParticle { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string Verb { get; set; } = string.Empty;
    public string VerbAndParticle { get; set; } = string.Empty;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Example, ExampleDetailsDto>().ReverseMap();
    }
}
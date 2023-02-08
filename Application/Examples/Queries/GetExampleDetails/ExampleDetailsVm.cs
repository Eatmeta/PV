using Application.Common.Mappings;
using AutoMapper;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetExampleDetails;

public class ExampleDetailsVm : IMapWith<Example>
{
    public long ExampleId { get; set; }
    public string ExampleFull { get; set; } = string.Empty;
    public string ExampleFullUnderscore { get; set; } = string.Empty;
    public string ExampleVerb { get; set; } = string.Empty;
    public string ExampleParticle { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string Verb { get; set; } = string.Empty;
    public string VerbAndParticle { get; set; } = string.Empty;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Example, ExampleDetailsVm>()
            .ForMember(exampleVm => exampleVm.ExampleId,
                opt => opt.MapFrom(example => example.ExampleId))
            .ForMember(exampleVm => exampleVm.ExampleFull,
                opt => opt.MapFrom(example => example.ExampleFull))
            .ForMember(exampleVm => exampleVm.ExampleFullUnderscore,
                opt => opt.MapFrom(example => example.ExampleFullUnderscore))
            .ForMember(exampleVm => exampleVm.ExampleVerb,
                opt => opt.MapFrom(example => example.ExampleVerb))
            .ForMember(exampleVm => exampleVm.ExampleParticle,
                opt => opt.MapFrom(example => example.ExampleParticle))
            .ForMember(exampleVm => exampleVm.Meaning,
                opt => opt.MapFrom(example => example.Meaning))
            .ForMember(exampleVm => exampleVm.Verb,
                opt => opt.MapFrom(example => example.Verb))
            .ForMember(exampleVm => exampleVm.VerbAndParticle,
                opt => opt.MapFrom(example => example.VerbAndParticle));
    }
}
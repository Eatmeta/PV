using System.ComponentModel.DataAnnotations;
using Application.Common.Mappings;
using AutoMapper;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetExampleList;

public class ExampleLookupDto : IMapWith<Example>
{
    //[Required] - это просто для проверки обязательных полей. потом можно удалить
    [Required]
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
        profile.CreateMap<Example, ExampleLookupDto>()
            .ForMember(exampleDto => exampleDto.ExampleId,
                opt => opt.MapFrom(example => example.ExampleId))
            .ForMember(exampleDto => exampleDto.ExampleFull,
                opt => opt.MapFrom(example => example.ExampleFull))
            .ForMember(exampleDto => exampleDto.ExampleFullUnderscore,
                opt => opt.MapFrom(example => example.ExampleFullUnderscore))
            .ForMember(exampleDto => exampleDto.ExampleVerb,
                opt => opt.MapFrom(example => example.ExampleVerb))
            .ForMember(exampleDto => exampleDto.ExampleParticle,
                opt => opt.MapFrom(example => example.ExampleParticle))
            .ForMember(exampleDto => exampleDto.Meaning,
                opt => opt.MapFrom(example => example.Meaning))
            .ForMember(exampleDto => exampleDto.Verb,
                opt => opt.MapFrom(example => example.Verb))
            .ForMember(exampleDto => exampleDto.VerbAndParticle,
                opt => opt.MapFrom(example => example.VerbAndParticle));
    }
}
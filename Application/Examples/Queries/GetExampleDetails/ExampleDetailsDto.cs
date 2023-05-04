using Application.Common.Mappings;
using AutoMapper;
using Domain.Models;

namespace Application.Examples.Queries.GetExampleDetails;

public class ExampleDetailsDto : IMapWith<Example>
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public int VerbId { get; set; }
    public Verb Verb { get; set; }
    public int ParticleId { get; set; }
    public Particle Particle { get; set; }
    public int MeaningId { get; set; }
    public Meaning Meaning { get; set; }
    public int PhrasalVerbId { get; set; }
    public PhrasalVerb PhrasalVerb { get; set; }
    public string ExampleVerb { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Example, ExampleDetailsDto>().ReverseMap();
    }
}
using Application.Common.Mappings;
using Application.Examples.Commands.CreateExampleAttempt;
using AutoMapper;

namespace Api.Models;

public class CreateExampleAttemptDto : IMapWith<CreateExampleAttemptCommand>
{
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public int ExampleId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public int Score { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateExampleAttemptDto, CreateExampleAttemptCommand>().ReverseMap();
    }
}
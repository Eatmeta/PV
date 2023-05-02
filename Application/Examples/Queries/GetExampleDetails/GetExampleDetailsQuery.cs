using MediatR;

namespace Application.Examples.Queries.GetExampleDetails;

public class GetExampleDetailsQuery : IRequest<ExampleDetailsDto>
{
    public int Id { get; init; }
}
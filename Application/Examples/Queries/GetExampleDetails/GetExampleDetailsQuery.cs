using MediatR;

namespace Application.Examples.Queries.GetExampleDetails;

public class GetExampleDetailsQuery : IRequest<ExampleDetailsDto>
{
    public Guid Id { get; init; }
}
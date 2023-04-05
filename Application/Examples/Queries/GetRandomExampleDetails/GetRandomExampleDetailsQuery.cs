using Application.Examples.Queries.GetExampleDetails;
using MediatR;

namespace Application.Examples.Queries.GetRandomExampleDetails;

public class GetRandomExampleDetailsQuery : IRequest<ExampleDetailsDto>
{
}
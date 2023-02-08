using MediatR;

namespace Application.Examples.Queries.GetExampleDetails;

public class GetExampleDetailsQuery : IRequest<ExampleDetailsVm>
{
    public long Id { get; set; }
}
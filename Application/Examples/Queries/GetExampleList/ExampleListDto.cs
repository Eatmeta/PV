using Application.Examples.Queries.GetExampleDetails;

namespace Application.Examples.Queries.GetExampleList;

public class ExampleListDto
{
    public IList<ExampleDetailsDto> Examples { get; set; }
}
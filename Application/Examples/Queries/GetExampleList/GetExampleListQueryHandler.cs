using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Examples.Queries.GetExampleList;

public class GetExampleListQueryHandler : IRequestHandler<GetExampleListQuery, ExampleListVm>
{
    private readonly IExamplesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetExampleListQueryHandler(IExamplesDbContext dbContext, IMapper mapper) =>
        (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ExampleListVm> Handle(GetExampleListQuery request, CancellationToken cancellationToken)
    {
        var examplesQuery = await _dbContext.Examples
            .ProjectTo<ExampleLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ExampleListVm {Examples = examplesQuery};
    }
}
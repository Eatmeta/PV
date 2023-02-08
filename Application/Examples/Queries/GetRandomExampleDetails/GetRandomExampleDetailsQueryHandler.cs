using Application.Common.Exceptions;
using Application.Examples.Queries.GetExampleDetails;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetRandomExampleDetails;

public class GetRandomExampleDetailsQueryHandler : IRequestHandler<GetRandomExampleDetailsQuery, ExampleDetailsVm>
{
    private readonly IExamplesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetRandomExampleDetailsQueryHandler(IExamplesDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ExampleDetailsVm> Handle(GetRandomExampleDetailsQuery request, CancellationToken cancellationToken)
    {
        var maxIndex = _dbContext.Examples.CountAsync(cancellationToken);
        var randomId = new Random().NextInt64(1, maxIndex.Result + 1);
        
        var entity = await _dbContext.Examples
            .FirstOrDefaultAsync(example => example.ExampleId.ToString() == randomId.ToString(), cancellationToken);

        if (entity == null || entity.ExampleId.ToString() != randomId.ToString())
            throw new NotFoundException(nameof(Example), randomId);

        return _mapper.Map<ExampleDetailsVm>(entity);
    }
}
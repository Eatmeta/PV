using Application.Common.Exceptions;
using Application.Examples.Queries.GetExampleDetails;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetRandomExampleDetails;

public class GetRandomExampleDetailsQueryHandler : IRequestHandler<GetRandomExampleDetailsQuery, ExampleDetailsDto>
{
    private readonly IExamplesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetRandomExampleDetailsQueryHandler(IExamplesDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ExampleDetailsDto> Handle(GetRandomExampleDetailsQuery request, CancellationToken cancellationToken)
    {
        var maxIndex = await _dbContext.Examples.CountAsync(cancellationToken);
        var randomId = new Random().Next(0, maxIndex);

        var entity = await _dbContext.Examples.Skip(randomId).Take(1)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
        if (entity == null)
            throw new NotFoundException(nameof(Example), randomId);

        return _mapper.Map<ExampleDetailsDto>(entity);
    }
}
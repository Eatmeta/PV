using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetExampleDetails;

public class GetExampleDetailsQueryHandler : IRequestHandler<GetExampleDetailsQuery, ExampleDetailsDto>
{
    private readonly IExamplesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetExampleDetailsQueryHandler(IExamplesDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ExampleDetailsDto> Handle(GetExampleDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Examples
            .FirstOrDefaultAsync(example => example.ExampleId == request.Id, cancellationToken);

        if (entity == null || entity.ExampleId != request.Id)
            throw new NotFoundException(nameof(Example), request.Id);

        return _mapper.Map<ExampleDetailsDto>(entity);
    }
}
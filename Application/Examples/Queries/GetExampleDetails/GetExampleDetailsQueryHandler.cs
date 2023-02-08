using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetExampleDetails;

public class GetExampleDetailsQueryHandler : IRequestHandler<GetExampleDetailsQuery, ExampleDetailsVm>
{
    private readonly IExamplesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetExampleDetailsQueryHandler(IExamplesDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ExampleDetailsVm> Handle(GetExampleDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Examples
            .FirstOrDefaultAsync(example => example.ExampleId.ToString() == request.Id.ToString(), cancellationToken);

        if (entity == null || entity.ExampleId.ToString() != request.Id.ToString())
            throw new NotFoundException(nameof(Example), request.Id);

        return _mapper.Map<ExampleDetailsVm>(entity);
    }
}
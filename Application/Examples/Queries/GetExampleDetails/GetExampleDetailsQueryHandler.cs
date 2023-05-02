using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            .FirstOrDefaultAsync(example => example.Id == request.Id, cancellationToken);

        if (entity == null || entity.Id != request.Id)
            throw new NotFoundException(nameof(Example), request.Id);

        return _mapper.Map<ExampleDetailsDto>(entity);
    }
}
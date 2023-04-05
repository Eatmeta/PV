using Application.Common.Exceptions;
using Application.Examples.Queries.GetExampleDetails;
using Application.Examples.Queries.GetExampleList;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetFourRandomExampleDetails;

public class GetFourRandomExampleDetailsQueryHandler : IRequestHandler<GetFourRandomExampleDetailsQuery, ExampleListDto>
{
    private readonly IExamplesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetFourRandomExampleDetailsQueryHandler(IExamplesDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ExampleListDto> Handle(GetFourRandomExampleDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new List<ExampleDetailsDto>();
        var usedIds = new List<Guid>();
        var maxIndex = await _dbContext.Examples.CountAsync(cancellationToken);
        var random = new Random();

        while (result.Count != 4)
        {
            var randomId = random.Next(0, maxIndex);
            var entity = await _dbContext.Examples.Skip(randomId).Take(1)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (entity == null)
                throw new NotFoundException(nameof(Example), randomId);
            
            var dto = _mapper.Map<ExampleDetailsDto>(entity);

            if (usedIds.Contains(dto.ExampleId)) continue;

            usedIds.Add(dto.ExampleId);
            result.Add(dto);
        }
        return new ExampleListDto {Examples = result};
    }
}
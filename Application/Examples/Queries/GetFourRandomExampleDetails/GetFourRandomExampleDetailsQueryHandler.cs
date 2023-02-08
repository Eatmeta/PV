using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhrasalVerb.Domain;

namespace Application.Examples.Queries.GetFourRandomExampleDetails;

public class GetFourRandomExampleDetailsQueryHandler : IRequestHandler<GetFourRandomExampleDetailsQuery, FourExamplesVm>
{
    private readonly IExamplesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetFourRandomExampleDetailsQueryHandler(IExamplesDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<FourExamplesVm> Handle(GetFourRandomExampleDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new List<Example>();
        var usedIds = new List<long>();
        var maxIndex = _dbContext.Examples.CountAsync(cancellationToken);

        for (var i = 0; i < 4; i++)
        {
            var randomId = new Random().NextInt64(1, maxIndex.Result + 1);

            if (!usedIds.Contains(randomId))
            {
                usedIds.Add(randomId);
                var entity = await _dbContext.Examples
                    .FirstOrDefaultAsync(example => example.ExampleId.ToString() == randomId.ToString(), cancellationToken);
                
                if (entity == null || entity.ExampleId.ToString() != randomId.ToString())
                    throw new NotFoundException(nameof(Example), randomId);
                
                result.Add(entity);
                continue;
            }
            i--;
        }
        return new FourExamplesVm {Examples = result};
    }
}
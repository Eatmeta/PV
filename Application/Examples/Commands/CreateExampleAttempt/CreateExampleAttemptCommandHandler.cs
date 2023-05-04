using Application.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Examples.Commands.CreateExampleAttempt;

public class CreateExampleAttemptCommandHandler : IRequestHandler<CreateExampleAttemptCommand, long>
{
    private readonly IExamplesDbContext _dbContext;

    public CreateExampleAttemptCommandHandler(IExamplesDbContext dbContext)
        => _dbContext = dbContext;
    
    public async Task<long> Handle(CreateExampleAttemptCommand request, CancellationToken cancellationToken)
    {
        var exampleAttempt = new ExampleAttempt
        {
            Date = DateTime.UtcNow,
            UserId = request.UserId,
            ExampleId = request.ExampleId,
            Answer = request.Answer,
            Score = request.Score
        };
        
        await _dbContext.ExampleAttempts.AddAsync(exampleAttempt, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return exampleAttempt.Id;
    }
}
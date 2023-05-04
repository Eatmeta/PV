using MediatR;

namespace Application.Examples.Commands.CreateExampleAttempt;

public class CreateExampleAttemptCommand : IRequest<long>
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public int ExampleId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public int Score { get; set; }
}
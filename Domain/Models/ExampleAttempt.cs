namespace Domain.Models;

public class ExampleAttempt
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public int ExampleId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public int Score { get; set; }
}
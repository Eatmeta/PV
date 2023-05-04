namespace BlazorServerApp.Models;

public class CreateExampleAttemptDto
{
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public int ExampleId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public int Score { get; set; }
}
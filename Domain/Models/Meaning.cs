namespace Domain.Models;

public class Meaning
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public virtual ICollection<Example> Examples { get; private set; } = new List<Example>();
}
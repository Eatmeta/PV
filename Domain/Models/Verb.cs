namespace Domain.Models;

public class Verb
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public int Productivity { get; set; }
    public virtual ICollection<PhrasalVerb> PhrasalVerbs { get; private set; } = new List<PhrasalVerb>();
    public virtual ICollection<Example> Examples { get; private set; } = new List<Example>();
}
namespace Domain.Models;

public class Particle
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public int Frequency { get; set; }
    public virtual ICollection<PhrasalVerb> PhrasalVerbs { get; private set; } = new List<PhrasalVerb>();
    public virtual ICollection<Example> Examples { get; private set; } = new List<Example>();
}
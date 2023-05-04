namespace Domain.Models;

public class PhrasalVerb
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public int VerbId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Verb Verb { get; set; }
    public int ParticleId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Particle Particle { get; set; }
    public int Frequency { get; set; }
    public virtual ICollection<Example> Examples { get; private set; } = new List<Example>();
}
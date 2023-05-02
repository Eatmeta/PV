namespace Domain.Models;

public class Example
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public int VerbId { get; set; }
    public virtual Verb Verb { get; set; }
    public int ParticleId { get; set; }
    public virtual Particle Particle { get; set; }
    public int MeaningId { get; set; }
    public virtual Meaning Meaning { get; set; }
    public int PhrasalVerbId { get; set; }
    public virtual PhrasalVerb PhrasalVerb { get; set; }
}
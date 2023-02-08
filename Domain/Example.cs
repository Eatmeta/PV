namespace PhrasalVerb.Domain;

public class Example
{
    public long ExampleId { get; set; }
    public string ExampleFull { get; set; } = string.Empty;
    public string ExampleFullUnderscore { get; set; } = string.Empty;
    public string ExampleVerb { get; set; } = string.Empty;
    public string ExampleParticle { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string Verb { get; set; } = string.Empty;
    public string VerbAndParticle { get; set; } = string.Empty;
}
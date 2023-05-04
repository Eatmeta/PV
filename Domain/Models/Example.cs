﻿namespace Domain.Models;

public class Example
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public int VerbId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Verb Verb { get; set; }
    public int ParticleId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Particle Particle { get; set; }
    public int MeaningId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Meaning Meaning { get; set; }
    public int PhrasalVerbId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual PhrasalVerb PhrasalVerb { get; set; }
    
    public string ExampleVerb { get; set; } = string.Empty;
}
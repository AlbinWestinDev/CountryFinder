using System;

public class CountriesDto
{
    public Guid Id { get; set; }
    public string? SenastÄndradAv { get; set; }
    public DateTime SenastÄndrad { get; set; }
    public string? SkapadAv { get; set; }
    public DateTime Skapad { get; set; }
    public int Version { get; set; }
    public string? Namn { get; set; }

}
namespace MicroWaveAPI.Dtos;

public class CreateHeatingModeDto
{
    public string? Name { get; set; }
    public string? Food { get; set; }
    public int Time { get; set; }
    public int Power { get; set; }
    public string? Instructions { get; set; }
    
    public char? CharIndicator { get; set; }
}
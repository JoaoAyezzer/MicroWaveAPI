namespace MicroWaveAPI.Dtos;

public class HeatingProcessDto
{
    public int Power { get; set; }
    public int TimeRemaining { get; set; }
    public bool IsPaused { get; set; }
    public bool IsCanceled { get; set; }
    public string StringIndicator { get; set; }
}
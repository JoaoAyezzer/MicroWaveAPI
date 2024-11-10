using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroWaveAPI.Models;

[Table("heating_mode")]
public class HeatingMode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Food { get; set; }
    public int Time { get; set; }
    public int Power { get; set; }
    public string? Instructions { get; set; }
}
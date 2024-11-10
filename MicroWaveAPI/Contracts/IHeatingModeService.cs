using MicroWaveAPI.Models;

namespace MicroWaveAPI.Contracts;

public interface IHeatingModeService
{
    Task<IEnumerable<HeatingMode>> GetAll();
    
}
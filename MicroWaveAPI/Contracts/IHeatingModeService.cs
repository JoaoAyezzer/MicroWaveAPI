using MicroWaveAPI.Dtos;
using MicroWaveAPI.Models;

namespace MicroWaveAPI.Contracts;

public interface IHeatingModeService
{
    Task<IEnumerable<HeatingMode>> GetAllAsync();
    Task<HeatingMode> GetByIdAsync(long id);
    Task<HeatingMode> CreateAsync(CreateHeatingModeDto dto);
    Task<HeatingMode> UpdateAsync(HeatingMode heatingMode);
    Task<bool> DeleteAsync(long id);


}
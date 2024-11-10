using Microsoft.EntityFrameworkCore;
using MicroWaveAPI.Contracts;
using MicroWaveAPI.Data;
using MicroWaveAPI.Data.Mappers;
using MicroWaveAPI.Dtos;
using MicroWaveAPI.Exceptions;
using MicroWaveAPI.Models;

namespace MicroWaveAPI.Services;

public class HeatingModeService(ApplicationDbContext context) : IHeatingModeService
{
    public async Task<IEnumerable<HeatingMode>> GetAllAsync()
    {
        return await context.HeatingModeCollection.ToListAsync();
    }

    public async Task<HeatingMode> GetByIdAsync(long id)
    {
        var heatingMode = await context.FindAsync<HeatingMode>(id);
        if (heatingMode == null)
        {
            throw new ObjectNotFoundException($"Heating Mode not found with id: {id}");
        }

        return heatingMode;
    }

    public async Task<HeatingMode> CreateAsync(CreateHeatingModeDto dto)
    {
        var heatingMode = HeatingModeMapper.MapToEntity(dto);
        
        context.HeatingModeCollection.Add(heatingMode);
        await context.SaveChangesAsync();
        return heatingMode;
    }

    public async Task<HeatingMode> UpdateAsync(HeatingMode heatingMode)
    {
        var entity = await GetByIdAsync(heatingMode.Id);
        
        HeatingModeMapper.ValidateEntity(heatingMode);

        entity.Food = heatingMode.Food;
        entity.Instructions = heatingMode.Instructions;
        entity.Name = heatingMode.Name;
        entity.Power = heatingMode.Power;
        entity.Time = heatingMode.Time;
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        try
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new BadRequestException("There was an error deleting the resource. Please check and try again.");
        }

        return true;
    }
}
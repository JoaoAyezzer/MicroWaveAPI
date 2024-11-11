using MicroWaveAPI.Dtos;
using MicroWaveAPI.Exceptions;
using MicroWaveAPI.Models;

namespace MicroWaveAPI.Data.Mappers;

public static class HeatingModeMapper
{
    public static HeatingMode MapToEntity(CreateHeatingModeDto dto)
    {
        // Validação dos campos
        ValidateDto(dto);

        return new HeatingMode
        {
            Name = dto.Name,
            Food = dto.Food,
            Time = dto.Time,
            Power = dto.Power,
            Instructions = dto.Instructions
        };
    }

    private static void ValidateDto(CreateHeatingModeDto dto)
    {
        // Verificar se os campos obrigatórios estão presentes
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name))
            errors.Add("Name is required.");

        if (string.IsNullOrWhiteSpace(dto.Food))
            errors.Add("Food is required.");

        if (dto.Time <= 0)
            errors.Add("Time must be greater than 0.");

        if (dto.Power <= 0)
            errors.Add("Power must be greater than 0.");
        switch (dto.CharIndicator)
        {
            case null:
                errors.Add("CharIndicator is required.");
                break;
            case '.':
                errors.Add("Reserved character");
                break;
        }
        // Se houver erros, lançar uma exceção
        if (errors.Count != 0)
            throw new BadRequestException(string.Join(" ", errors));
        
    }
    public static void ValidateEntity(HeatingMode heatingMode)
    {
        // Verificar se os campos obrigatórios estão presentes
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(heatingMode.Name))
            errors.Add("Name is required.");    

        if (string.IsNullOrWhiteSpace(heatingMode.Food))
            errors.Add("Food is required.");
        
        if (heatingMode.CharIndicator == null)
            errors.Add("CharIndicator is required.");
        
        if (heatingMode.Time <= 0)
            errors.Add("Time must be greater than 0.");

        if (heatingMode.Power <= 0)
            errors.Add("Power must be greater than 0.");
        switch (heatingMode.CharIndicator)
        {
            case null:
                errors.Add("CharIndicator is required.");
                break;
            case '.':
                errors.Add("Reserved character");
                break;
        }
        // Se houver erros, lançar uma exceção
        if (errors.Count != 0)
            throw new BadRequestException(string.Join(" ", errors));
    }
    
}
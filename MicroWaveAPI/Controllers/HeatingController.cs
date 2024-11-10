using Microsoft.AspNetCore.Mvc;
using MicroWaveAPI.Contracts;
using MicroWaveAPI.Dtos;
using MicroWaveAPI.Models;

namespace MicroWaveAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HeatingController(IHeatingModeService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HeatingMode>>> GetAll()
    {
        var result = await service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:long}", Name = "GetById")]
    public async Task<ActionResult<HeatingMode>> GetById(long id)
    {
        var result = await service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateHeatingModeDto dto)
    {
        var result = await service.CreateAsync(dto);
        
        var uri = Url.Link("GetById", new { id = result.Id });

        return Created(uri, result);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] HeatingMode heatingMode)
    {
        await service.UpdateAsync(heatingMode);

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteById(long id)
    {
        await service.DeleteAsync(id);

        return NoContent();
    }
    
    
}
using ConfigurationManager.ConfigurationManager.API.Models;
using ConfigurationManager.ConfigurationManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationManager.ConfigurationManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationsController(IConfigurationService configurationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConfigurationDto>>> GetAllConfigurations(Guid? userId = null,
        string? name = null, DateTime? dateCreated = null)
    {
        var configurations = await configurationService.GetAllConfigurationsAsync(userId, name, dateCreated);
        return Ok(configurations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConfigurationDto>> GetConfigurationById(Guid id)
    {
        var configuration = await configurationService.GetConfigurationByIdAsync(id);
        if (configuration == null)
        {
            return NotFound();
        }

        return Ok(configuration);
    }

    [HttpGet("{configurationId}/versions/{configurationVersionId}")]
    public async Task<ActionResult<ConfigurationDto>> GetConfigurationVersion(Guid configurationId,
        Guid configurationVersionId)
    {
        var configuration =
            await configurationService.GetConfigurationVersionAsync(configurationId, configurationVersionId);
        if (configuration == null)
        {
            return NotFound();
        }

        return Ok(configuration);
    }

    [HttpPost]
    public async Task<ActionResult<ConfigurationDto>> CreateConfiguration([FromBody] ConfigurationCreateDto createDto)
    {
        try
        {
            var configuration = await configurationService.CreateConfigurationAsync(createDto);
            if (configuration.IsSuccess)
            {
                return CreatedAtAction(nameof(GetConfigurationById), new { id = configuration.Result.Id },
                    configuration);
            }

            return BadRequest(configuration.ErrorMessage);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{configurationId}")]
    public async Task<IActionResult> UpdateConfiguration(Guid configurationId, ConfigurationUpdateDto updateDto)
    {
        try
        {
            var configuration = await configurationService.UpdateConfigurationAsync(configurationId, updateDto);
            return Ok(configuration);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{configurationId}")]
    public async Task<IActionResult> DeleteConfiguration(Guid configurationId)
    {
        try
        {
            await configurationService.DeleteConfigurationAsync(configurationId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

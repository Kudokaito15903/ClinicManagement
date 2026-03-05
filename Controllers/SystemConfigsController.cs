using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for system configuration values.
/// </summary>
[ApiController]
[Route("api/configs")]
public class SystemConfigsController : ControllerBase
{
    private readonly SystemConfigService _configService;
    public SystemConfigsController(SystemConfigService configService) => _configService = configService;

    /// <summary>
    /// Get all system configs.
    /// </summary>
    /// <response code="200">Returns all configuration items.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _configService.GetAllAsync());

    /// <summary>
    /// Get one config by key.
    /// </summary>
    /// <param name="key">Configuration key.</param>
    /// <response code="200">Returns configuration item.</response>
    /// <response code="404">Configuration key was not found.</response>
    [HttpGet("{key}")]
    public async Task<IActionResult> GetByKey(string key) => Ok(await _configService.GetByKeyAsync(key));

    /// <summary>
    /// Update config value by key.
    /// </summary>
    /// <param name="key">Configuration key.</param>
    /// <param name="req">New config value payload.</param>
    /// <response code="200">Configuration updated successfully.</response>
    /// <response code="404">Configuration key was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPut("{key}")]
    public async Task<IActionResult> Update(string key, [FromBody] SystemConfigUpdateRequest req)
        => Ok(await _configService.UpdateAsync(key, req));
}

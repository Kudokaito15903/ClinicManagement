using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for managing chargeable medical services.
/// </summary>
[ApiController]
[Route("api/services")]
public class MedicalServicesController : ControllerBase
{
    private readonly MedicalServiceService _medicalServiceService;
    public MedicalServicesController(MedicalServiceService medicalServiceService)
        => _medicalServiceService = medicalServiceService;

    /// <summary>
    /// Get all medical services.
    /// </summary>
    /// <response code="200">Returns all medical services.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _medicalServiceService.FindAllAsync());

    /// <summary>
    /// Get a medical service by id.
    /// </summary>
    /// <param name="id">Service id.</param>
    /// <response code="200">Returns the medical service.</response>
    /// <response code="404">Medical service was not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) => Ok(await _medicalServiceService.FindByIdAsync(id));

    /// <summary>
    /// Create a medical service.
    /// </summary>
    /// <param name="req">Medical service information.</param>
    /// <response code="201">Medical service created successfully.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicalServiceRequest req)
    {
        var result = await _medicalServiceService.CreateAsync(req);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Update a medical service.
    /// </summary>
    /// <param name="id">Service id.</param>
    /// <param name="req">New medical service information.</param>
    /// <response code="200">Medical service updated successfully.</response>
    /// <response code="404">Medical service was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] MedicalServiceRequest req)
        => Ok(await _medicalServiceService.UpdateAsync(id, req));

    /// <summary>
    /// Delete a medical service.
    /// </summary>
    /// <param name="id">Service id.</param>
    /// <response code="204">Medical service deleted successfully.</response>
    /// <response code="404">Medical service was not found.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _medicalServiceService.DeleteAsync(id);
        return NoContent();
    }
}

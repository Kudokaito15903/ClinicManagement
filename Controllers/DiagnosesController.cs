using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for managing diagnosis master data.
/// </summary>
[ApiController]
[Route("api/diagnoses")]
public class DiagnosesController : ControllerBase
{
    private readonly DiagnosisService _diagnosisService;
    public DiagnosesController(DiagnosisService diagnosisService) => _diagnosisService = diagnosisService;

    /// <summary>
    /// Search diagnosis list by keyword.
    /// </summary>
    /// <param name="keyword">Keyword matched against code or name.</param>
    /// <response code="200">Returns the filtered diagnosis list.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        => Ok(await _diagnosisService.FindAllAsync(keyword));

    /// <summary>
    /// Get diagnosis details by id.
    /// </summary>
    /// <param name="id">Diagnosis id.</param>
    /// <response code="200">Returns diagnosis details.</response>
    /// <response code="404">Diagnosis was not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) => Ok(await _diagnosisService.FindByIdAsync(id));

    /// <summary>
    /// Create a diagnosis.
    /// </summary>
    /// <param name="req">Diagnosis information.</param>
    /// <response code="201">Diagnosis created successfully.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DiagnosisRequest req)
    {
        var result = await _diagnosisService.CreateAsync(req);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Update a diagnosis.
    /// </summary>
    /// <param name="id">Diagnosis id.</param>
    /// <param name="req">New diagnosis information.</param>
    /// <response code="200">Diagnosis updated successfully.</response>
    /// <response code="404">Diagnosis was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] DiagnosisRequest req)
        => Ok(await _diagnosisService.UpdateAsync(id, req));

    /// <summary>
    /// Delete a diagnosis.
    /// </summary>
    /// <param name="id">Diagnosis id.</param>
    /// <response code="204">Diagnosis deleted successfully.</response>
    /// <response code="404">Diagnosis was not found.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _diagnosisService.DeleteAsync(id);
        return NoContent();
    }
}

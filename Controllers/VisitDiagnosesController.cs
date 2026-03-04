using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for attaching diagnoses to a visit.
/// </summary>
[ApiController]
[Route("api/visits/{visitId}/diagnoses")]
public class VisitDiagnosesController : ControllerBase
{
    private readonly VisitDiagnosisService _service;
    public VisitDiagnosesController(VisitDiagnosisService service) => _service = service;

    /// <summary>
    /// Get diagnoses assigned to a visit.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <response code="200">Returns all diagnoses of the visit.</response>
    /// <response code="404">Visit was not found.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll(long visitId)
        => Ok(await _service.GetByVisitIdAsync(visitId));

    /// <summary>
    /// Add a diagnosis to a visit.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="req">Diagnosis payload.</param>
    /// <response code="201">Diagnosis added to visit.</response>
    /// <response code="404">Visit or diagnosis was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost]
    public async Task<IActionResult> Add(long visitId, [FromBody] VisitDiagnosisAddRequest req)
    {
        var result = await _service.AddAsync(visitId, req);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Remove a diagnosis from a visit.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="diagnosisId">Diagnosis id.</param>
    /// <response code="204">Diagnosis removed from visit.</response>
    /// <response code="404">Visit diagnosis mapping was not found.</response>
    [HttpDelete("{diagnosisId}")]
    public async Task<IActionResult> Remove(long visitId, long diagnosisId)
    {
        await _service.RemoveAsync(visitId, diagnosisId);
        return NoContent();
    }
}

using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for managing patients.
/// </summary>
[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly PatientService _patientService;
    private readonly VisitService _visitService;

    public PatientsController(PatientService patientService, VisitService visitService)
    {
        _patientService = patientService;
        _visitService = visitService;
    }

    /// <summary>
    /// Search patients by keyword.
    /// </summary>
    /// <param name="keyword">Keyword matched against code, name, or phone.</param>
    /// <response code="200">Returns matching patients.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        => Ok(await _patientService.FindAllAsync(keyword));

    /// <summary>
    /// Get patient details by id.
    /// </summary>
    /// <param name="id">Patient id.</param>
    /// <response code="200">Returns patient details.</response>
    /// <response code="404">Patient was not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(await _patientService.FindByIdAsync(id));

    /// <summary>
    /// Create a patient.
    /// </summary>
    /// <param name="req">Patient information.</param>
    /// <response code="201">Patient created successfully.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PatientCreateRequest req)
    {
        var result = await _patientService.CreateAsync(req);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Update a patient.
    /// </summary>
    /// <param name="id">Patient id.</param>
    /// <param name="req">New patient information.</param>
    /// <response code="200">Patient updated successfully.</response>
    /// <response code="404">Patient was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] PatientUpdateRequest req)
        => Ok(await _patientService.UpdateAsync(id, req));

    /// <summary>
    /// Delete a patient.
    /// </summary>
    /// <param name="id">Patient id.</param>
    /// <response code="204">Patient deleted successfully.</response>
    /// <response code="404">Patient was not found.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _patientService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Get visit history of a patient.
    /// </summary>
    /// <param name="id">Patient id.</param>
    /// <response code="200">Returns patient visit history.</response>
    /// <response code="404">Patient was not found.</response>
    [HttpGet("{id}/visits")]
    public async Task<IActionResult> GetVisitHistory(long id)
        => Ok(await _visitService.GetPatientHistoryAsync(id));
}

using ClinicManagement.DTOs.Requests;
using ClinicManagement.Entities;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for visit workflow and visit services.
/// </summary>
[ApiController]
[Route("api/visits")]
public class VisitsController : ControllerBase
{
    private readonly VisitService _visitService;
    private readonly VisitServiceService _visitServiceService;

    public VisitsController(VisitService visitService, VisitServiceService visitServiceService)
    {
        _visitService = visitService;
        _visitServiceService = visitServiceService;
    }

    /// <summary>
    /// Get visits with optional filters.
    /// </summary>
    /// <param name="date">Visit date filter in yyyy-MM-dd.</param>
    /// <param name="status">Visit status filter.</param>
    /// <param name="doctorId">Doctor id filter.</param>
    /// <response code="200">Returns filtered visits.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] VisitStatus? status,
        [FromQuery] long? doctorId)
        => Ok(await _visitService.GetAllAsync(date, status, doctorId));

    /// <summary>
    /// Create a new visit.
    /// </summary>
    /// <param name="req">Visit creation payload.</param>
    /// <response code="201">Visit created successfully.</response>
    /// <response code="404">Referenced patient, doctor, or room was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VisitCreateRequest req)
    {
        var result = await _visitService.CreateAsync(req);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Get visit details by id.
    /// </summary>
    /// <param name="id">Visit id.</param>
    /// <response code="200">Returns visit details.</response>
    /// <response code="404">Visit was not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(await _visitService.FindByIdAsync(id));

    /// <summary>
    /// Update visit information.
    /// </summary>
    /// <param name="id">Visit id.</param>
    /// <param name="req">Visit update payload.</param>
    /// <response code="200">Visit updated successfully.</response>
    /// <response code="404">Visit or related entities were not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] VisitUpdateRequest req)
        => Ok(await _visitService.UpdateAsync(id, req));

    /// <summary>
    /// Update only visit status.
    /// </summary>
    /// <param name="id">Visit id.</param>
    /// <param name="req">New status payload.</param>
    /// <response code="200">Status updated successfully.</response>
    /// <response code="404">Visit was not found.</response>
    /// <response code="400">Status transition is invalid.</response>
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] VisitStatusUpdateRequest req)
        => Ok(await _visitService.UpdateStatusAsync(id, req.Status));

    /// <summary>
    /// Get all services used in a visit.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <response code="200">Returns services attached to the visit.</response>
    /// <response code="404">Visit was not found.</response>
    [HttpGet("{visitId}/services")]
    public async Task<IActionResult> GetServices(long visitId)
        => Ok(await _visitServiceService.FindByVisitIdAsync(visitId));

    /// <summary>
    /// Add a service to a visit.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="req">Service and quantity payload.</param>
    /// <response code="201">Service added to visit.</response>
    /// <response code="404">Visit or service was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost("{visitId}/services")]
    public async Task<IActionResult> AddService(long visitId, [FromBody] VisitServiceAddRequest req)
    {
        var result = await _visitServiceService.AddServiceAsync(visitId, req);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Remove a service entry from a visit.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="vsId">Visit service item id.</param>
    /// <response code="204">Service removed from visit.</response>
    /// <response code="404">Visit service item was not found.</response>
    [HttpDelete("{visitId}/services/{vsId}")]
    public async Task<IActionResult> RemoveService(long visitId, long vsId)
    {
        await _visitServiceService.RemoveServiceAsync(visitId, vsId);
        return NoContent();
    }
}

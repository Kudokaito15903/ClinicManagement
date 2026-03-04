using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for managing doctors.
/// </summary>
[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly DoctorService _doctorService;
    public DoctorsController(DoctorService doctorService) => _doctorService = doctorService;

    /// <summary>
    /// Get all doctors.
    /// </summary>
    /// <response code="200">Returns the list of doctors.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _doctorService.FindAllAsync());

    /// <summary>
    /// Get a doctor by id.
    /// </summary>
    /// <param name="id">Doctor id.</param>
    /// <response code="200">Returns the doctor.</response>
    /// <response code="404">Doctor was not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) => Ok(await _doctorService.FindByIdAsync(id));

    /// <summary>
    /// Create a doctor.
    /// </summary>
    /// <param name="req">Doctor information.</param>
    /// <response code="201">Doctor created successfully.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DoctorRequest req)
    {
        var result = await _doctorService.CreateAsync(req);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Update a doctor.
    /// </summary>
    /// <param name="id">Doctor id.</param>
    /// <param name="req">New doctor information.</param>
    /// <response code="200">Doctor updated successfully.</response>
    /// <response code="404">Doctor was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] DoctorRequest req)
        => Ok(await _doctorService.UpdateAsync(id, req));

    /// <summary>
    /// Delete a doctor.
    /// </summary>
    /// <param name="id">Doctor id.</param>
    /// <response code="204">Doctor deleted successfully.</response>
    /// <response code="404">Doctor was not found.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _doctorService.DeleteAsync(id);
        return NoContent();
    }
}

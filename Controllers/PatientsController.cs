using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        => Ok(await _patientService.FindAllAsync(keyword));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(await _patientService.FindByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PatientCreateRequest req)
    {
        var result = await _patientService.CreateAsync(req);
        return StatusCode(201, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] PatientUpdateRequest req)
        => Ok(await _patientService.UpdateAsync(id, req));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _patientService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Lịch sử khám của bệnh nhân</summary>
    [HttpGet("{id}/visits")]
    public async Task<IActionResult> GetVisitHistory(long id)
        => Ok(await _visitService.GetPatientHistoryAsync(id));
}

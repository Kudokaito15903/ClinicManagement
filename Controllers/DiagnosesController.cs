using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/diagnoses")]
public class DiagnosesController : ControllerBase
{
    private readonly DiagnosisService _diagnosisService;
    public DiagnosesController(DiagnosisService diagnosisService) => _diagnosisService = diagnosisService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        => Ok(await _diagnosisService.FindAllAsync(keyword));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) => Ok(await _diagnosisService.FindByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DiagnosisRequest req)
    {
        var result = await _diagnosisService.CreateAsync(req);
        return StatusCode(201, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] DiagnosisRequest req)
        => Ok(await _diagnosisService.UpdateAsync(id, req));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _diagnosisService.DeleteAsync(id);
        return NoContent();
    }
}

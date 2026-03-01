using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VisitCreateRequest req)
    {
        var result = await _visitService.CreateAsync(req);
        return StatusCode(201, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(await _visitService.FindByIdAsync(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] VisitUpdateRequest req)
        => Ok(await _visitService.UpdateAsync(id, req));

    // ─── Visit Services ───────────────────────────────────────────────────────

    [HttpGet("{visitId}/services")]
    public async Task<IActionResult> GetServices(long visitId)
        => Ok(await _visitServiceService.FindByVisitIdAsync(visitId));

    [HttpPost("{visitId}/services")]
    public async Task<IActionResult> AddService(long visitId, [FromBody] VisitServiceAddRequest req)
    {
        var result = await _visitServiceService.AddServiceAsync(visitId, req);
        return StatusCode(201, result);
    }

    [HttpDelete("{visitId}/services/{vsId}")]
    public async Task<IActionResult> RemoveService(long visitId, long vsId)
    {
        await _visitServiceService.RemoveServiceAsync(visitId, vsId);
        return NoContent();
    }
}

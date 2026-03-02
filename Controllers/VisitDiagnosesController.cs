using ClinicManagement.DTOs.Requests;
using ClinicManagement.Entities;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/visits/{visitId}/diagnoses")]
public class VisitDiagnosesController : ControllerBase
{
    private readonly VisitDiagnosisService _service;
    public VisitDiagnosesController(VisitDiagnosisService service) => _service = service;

    /// <summary>Danh sách chẩn đoán của lượt khám</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(long visitId)
        => Ok(await _service.GetByVisitIdAsync(visitId));

    /// <summary>Thêm chẩn đoán vào lượt khám</summary>
    [HttpPost]
    public async Task<IActionResult> Add(long visitId, [FromBody] VisitDiagnosisAddRequest req)
    {
        var result = await _service.AddAsync(visitId, req);
        return StatusCode(201, result);
    }

    /// <summary>Xóa chẩn đoán khỏi lượt khám</summary>
    [HttpDelete("{diagnosisId}")]
    public async Task<IActionResult> Remove(long visitId, long diagnosisId)
    {
        await _service.RemoveAsync(visitId, diagnosisId);
        return NoContent();
    }
}

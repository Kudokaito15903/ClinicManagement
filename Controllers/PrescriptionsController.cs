using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs quản lý đơn thuốc theo lần khám.
/// Đơn thuốc (prescription) gắn 1-1 với visit.
/// </summary>
[ApiController]
[Route("api/visits/{visitId:long}/prescription")]
public class PrescriptionsController : ControllerBase
{
    private readonly PrescriptionService _service;
    public PrescriptionsController(PrescriptionService service) => _service = service;

    /// <summary>Lấy đơn thuốc của lần khám. Trả 204 nếu chưa có đơn.</summary>
    /// <param name="visitId">Visit id.</param>
    [HttpGet]
    public async Task<IActionResult> Get(long visitId)
    {
        var result = await _service.GetByVisitAsync(visitId);
        return result is null ? NoContent() : Ok(result);
    }

    /// <summary>Tạo đơn thuốc cho lần khám (chỉ tạo được 1 đơn / visit).</summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="req">Ghi chú của bác sĩ.</param>
    /// <response code="201">Đơn thuốc tạo thành công.</response>
    /// <response code="400">Lần khám đã có đơn thuốc.</response>
    /// <response code="404">Visit không tồn tại.</response>
    [HttpPost]
    public async Task<IActionResult> Create(long visitId, [FromBody] PrescriptionCreateRequest req)
    {
        var result = await _service.CreateAsync(visitId, req);
        return StatusCode(201, result);
    }

    /// <summary>Cập nhật ghi chú đơn thuốc.</summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="req">Note mới.</param>
    /// <response code="404">Visit chưa có đơn thuốc.</response>
    [HttpPut]
    public async Task<IActionResult> Update(long visitId, [FromBody] PrescriptionUpdateRequest req)
        => Ok(await _service.UpdateAsync(visitId, req));

    /// <summary>Xoá toàn bộ đơn thuốc (và các mục) của lần khám.</summary>
    /// <param name="visitId">Visit id.</param>
    [HttpDelete]
    public async Task<IActionResult> Delete(long visitId)
    {
        await _service.DeleteAsync(visitId);
        return NoContent();
    }

    // ─── Items ─────────────────────────────────────────────────────────────────

    /// <summary>Thêm một loại thuốc vào đơn.</summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="req">Thông tin thuốc cần thêm.</param>
    /// <response code="201">Thuốc thêm thành công.</response>
    /// <response code="400">Thuốc đã có trong đơn hoặc không còn sử dụng.</response>
    /// <response code="404">Visit chưa có đơn hoặc thuốc không tồn tại.</response>
    [HttpPost("items")]
    public async Task<IActionResult> AddItem(long visitId, [FromBody] PrescriptionItemAddRequest req)
    {
        var result = await _service.AddItemAsync(visitId, req);
        return StatusCode(201, result);
    }

    /// <summary>Cập nhật số lượng / hướng dẫn dùng của một mục trong đơn.</summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="itemId">PrescriptionItem id.</param>
    /// <param name="req">Thông tin cập nhật.</param>
    [HttpPut("items/{itemId:long}")]
    public async Task<IActionResult> UpdateItem(long visitId, long itemId, [FromBody] PrescriptionItemUpdateRequest req)
        => Ok(await _service.UpdateItemAsync(visitId, itemId, req));

    /// <summary>Xoá một mục thuốc khỏi đơn.</summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="itemId">PrescriptionItem id.</param>
    [HttpDelete("items/{itemId:long}")]
    public async Task<IActionResult> RemoveItem(long visitId, long itemId)
        => Ok(await _service.RemoveItemAsync(visitId, itemId));
}

using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs quản lý danh mục thuốc.
/// </summary>
[ApiController]
[Route("api/medicines")]
public class MedicinesController : ControllerBase
{
    private readonly MedicineService _service;
    public MedicinesController(MedicineService service) => _service = service;

    /// <summary>Lấy danh sách thuốc, hỗ trợ tìm kiếm và lọc theo trạng thái.</summary>
    /// <param name="search">Từ khoá tìm theo tên, mã, hoặc hoạt chất.</param>
    /// <param name="activeOnly">true = chỉ lấy thuốc đang sử dụng.</param>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] bool? activeOnly)
        => Ok(await _service.GetAllAsync(search, activeOnly));

    /// <summary>Lấy chi tiết một loại thuốc.</summary>
    /// <param name="id">Medicine id.</param>
    /// <response code="404">Thuốc không tồn tại.</response>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(await _service.GetByIdAsync(id));

    /// <summary>Thêm thuốc mới vào danh mục.</summary>
    /// <param name="req">Thông tin thuốc.</param>
    /// <response code="201">Thuốc được tạo thành công.</response>
    /// <response code="400">Dữ liệu không hợp lệ.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicineCreateRequest req)
    {
        var result = await _service.CreateAsync(req);
        return StatusCode(201, result);
    }

    /// <summary>Cập nhật thông tin thuốc (không đổi được mã).</summary>
    /// <param name="id">Medicine id.</param>
    /// <param name="req">Thông tin cập nhật.</param>
    /// <response code="404">Thuốc không tồn tại.</response>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] MedicineUpdateRequest req)
        => Ok(await _service.UpdateAsync(id, req));

    /// <summary>Kích hoạt thuốc.</summary>
    /// <param name="id">Medicine id.</param>
    [HttpPatch("{id:long}/activate")]
    public async Task<IActionResult> Activate(long id)
        => Ok(await _service.SetActiveAsync(id, true));

    /// <summary>Ngừng sử dụng thuốc (soft-disable).</summary>
    /// <param name="id">Medicine id.</param>
    [HttpPatch("{id:long}/deactivate")]
    public async Task<IActionResult> Deactivate(long id)
        => Ok(await _service.SetActiveAsync(id, false));
}

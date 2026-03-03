using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/configs")]
public class SystemConfigsController : ControllerBase
{
    private readonly SystemConfigService _configService;
    public SystemConfigsController(SystemConfigService configService) => _configService = configService;

    /// <summary>Lấy toàn bộ cấu hình hệ thống</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _configService.GetAllAsync());

    /// <summary>Lấy một config theo key</summary>
    [HttpGet("{key}")]
    public async Task<IActionResult> GetByKey(string key) => Ok(await _configService.GetByKeyAsync(key));

    /// <summary>Cập nhật giá trị config (admin)</summary>
    [HttpPut("{key}")]
    public async Task<IActionResult> Update(string key, [FromBody] SystemConfigUpdateRequest req)
        => Ok(await _configService.UpdateAsync(key, req));
}

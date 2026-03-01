using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ReportService _reportService;
    public ReportsController(ReportService reportService) => _reportService = reportService;

    /// <summary>Thống kê tổng quan phòng khám</summary>
    [HttpGet("api/statistics")]
    public async Task<IActionResult> GetStatistics()
        => Ok(await _reportService.GetStatisticsAsync());

    /// <summary>Báo cáo doanh thu theo khoảng ngày: ?from=2025-01-01&amp;to=2025-01-31</summary>
    [HttpGet("api/reports/revenue")]
    public async Task<IActionResult> GetRevenueReport(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to)
        => Ok(await _reportService.GetRevenueReportAsync(from, to));

    /// <summary>Chi tiết đầy đủ 1 lần khám: visit + dịch vụ + tổng tiền</summary>
    [HttpGet("api/visits/{id}/detail")]
    public async Task<IActionResult> GetVisitDetail(long id)
        => Ok(await _reportService.GetVisitDetailAsync(id));
}

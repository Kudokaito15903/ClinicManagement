using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for dashboard statistics and reports.
/// </summary>
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ReportService _reportService;
    public ReportsController(ReportService reportService) => _reportService = reportService;

    /// <summary>
    /// Get clinic summary statistics for dashboard.
    /// </summary>
    /// <response code="200">Returns dashboard statistics.</response>
    [HttpGet("api/statistics")]
    public async Task<IActionResult> GetStatistics()
        => Ok(await _reportService.GetStatisticsAsync());

    /// <summary>
    /// Get revenue report by date range.
    /// </summary>
    /// <param name="from">Start date in yyyy-MM-dd.</param>
    /// <param name="to">End date in yyyy-MM-dd.</param>
    /// <response code="200">Returns revenue report grouped by day.</response>
    /// <response code="400">Date range is invalid.</response>
    [HttpGet("api/reports/revenue")]
    public async Task<IActionResult> GetRevenueReport(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to)
        => Ok(await _reportService.GetRevenueReportAsync(from, to));

    /// <summary>
    /// Get complete details of one visit.
    /// </summary>
    /// <param name="id">Visit id.</param>
    /// <response code="200">Returns visit details with services and totals.</response>
    /// <response code="404">Visit was not found.</response>
    [HttpGet("api/visits/{id}/detail")]
    public async Task<IActionResult> GetVisitDetail(long id)
        => Ok(await _reportService.GetVisitDetailAsync(id));
}

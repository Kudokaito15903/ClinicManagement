using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for bill and receipt generation.
/// </summary>
[ApiController]
[Route("api/visits")]
public class BillingController : ControllerBase
{
    private readonly BillingService _billingService;
    public BillingController(BillingService billingService) => _billingService = billingService;

    /// <summary>
    /// Get bill data in JSON format.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <response code="200">Returns bill data.</response>
    /// <response code="404">Visit or payment information was not found.</response>
    [HttpGet("{visitId}/bill")]
    public async Task<IActionResult> GetBill(long visitId)
        => Ok(await _billingService.GetBillAsync(visitId));

    /// <summary>
    /// Get printable receipt in HTML format.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <response code="200">Returns HTML receipt content.</response>
    /// <response code="404">Visit or payment information was not found.</response>
    [HttpGet("{visitId}/receipt")]
    public async Task<ContentResult> GetReceiptHtml(long visitId)
    {
        var html = await _billingService.GetReceiptHtmlAsync(visitId);
        return new ContentResult { Content = html, ContentType = "text/html", StatusCode = 200 };
    }
}

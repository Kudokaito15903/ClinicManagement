using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/visits")]
public class BillingController : ControllerBase
{
    private readonly BillingService _billingService;
    public BillingController(BillingService billingService) => _billingService = billingService;

    /// <summary>Phiếu thu dạng JSON</summary>
    [HttpGet("{visitId}/bill")]
    public async Task<IActionResult> GetBill(long visitId)
        => Ok(await _billingService.GetBillAsync(visitId));

    /// <summary>Phiếu thu dạng HTML (có thể in trực tiếp từ trình duyệt)</summary>
    [HttpGet("{visitId}/receipt")]
    public async Task<ContentResult> GetReceiptHtml(long visitId)
    {
        var html = await _billingService.GetReceiptHtmlAsync(visitId);
        return new ContentResult { Content = html, ContentType = "text/html", StatusCode = 200 };
    }
}

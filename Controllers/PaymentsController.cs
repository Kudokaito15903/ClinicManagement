using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/visits/{visitId}/payment")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    public PaymentsController(PaymentService paymentService) => _paymentService = paymentService;

    /// <summary>Xem thông tin thanh toán của lượt khám</summary>
    [HttpGet]
    public async Task<IActionResult> Get(long visitId)
        => Ok(await _paymentService.GetByVisitIdAsync(visitId));

    /// <summary>Thu tiền — tạo phiếu thanh toán cho lượt khám</summary>
    [HttpPost]
    public async Task<IActionResult> Create(long visitId, [FromBody] PaymentCreateRequest req)
    {
        var result = await _paymentService.CreateAsync(visitId, req);
        return StatusCode(201, result);
    }
}

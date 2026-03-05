using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for payment operations of a visit.
/// </summary>
[ApiController]
[Route("api/visits/{visitId}/payment")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    public PaymentsController(PaymentService paymentService) => _paymentService = paymentService;

    /// <summary>
    /// Get payment information of a visit.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <response code="200">Returns payment information.</response>
    /// <response code="404">Visit payment was not found.</response>
    [HttpGet]
    public async Task<IActionResult> Get(long visitId)
        => Ok(await _paymentService.GetByVisitIdAsync(visitId));

    /// <summary>
    /// Create a payment receipt for a visit.
    /// </summary>
    /// <param name="visitId">Visit id.</param>
    /// <param name="req">Payment payload including discount and method.</param>
    /// <response code="201">Payment created successfully.</response>
    /// <response code="404">Visit was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost]
    public async Task<IActionResult> Create(long visitId, [FromBody] PaymentCreateRequest req)
    {
        var result = await _paymentService.CreateAsync(visitId, req);
        return StatusCode(201, result);
    }
}

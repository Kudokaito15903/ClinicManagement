using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/services")]
public class MedicalServicesController : ControllerBase
{
    private readonly MedicalServiceService _medicalServiceService;
    public MedicalServicesController(MedicalServiceService medicalServiceService)
        => _medicalServiceService = medicalServiceService;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _medicalServiceService.FindAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) => Ok(await _medicalServiceService.FindByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicalServiceRequest req)
    {
        var result = await _medicalServiceService.CreateAsync(req);
        return StatusCode(201, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] MedicalServiceRequest req)
        => Ok(await _medicalServiceService.UpdateAsync(id, req));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _medicalServiceService.DeleteAsync(id);
        return NoContent();
    }
}

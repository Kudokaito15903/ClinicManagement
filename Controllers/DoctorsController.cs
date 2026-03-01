using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly DoctorService _doctorService;
    public DoctorsController(DoctorService doctorService) => _doctorService = doctorService;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _doctorService.FindAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) => Ok(await _doctorService.FindByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DoctorRequest req)
    {
        var result = await _doctorService.CreateAsync(req);
        return StatusCode(201, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] DoctorRequest req)
        => Ok(await _doctorService.UpdateAsync(id, req));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _doctorService.DeleteAsync(id);
        return NoContent();
    }
}

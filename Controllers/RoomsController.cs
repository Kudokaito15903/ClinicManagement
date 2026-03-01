using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly RoomService _roomService;
    public RoomsController(RoomService roomService) => _roomService = roomService;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _roomService.FindAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) => Ok(await _roomService.FindByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoomRequest req)
    {
        var result = await _roomService.CreateAsync(req);
        return StatusCode(201, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] RoomRequest req)
        => Ok(await _roomService.UpdateAsync(id, req));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _roomService.DeleteAsync(id);
        return NoContent();
    }
}

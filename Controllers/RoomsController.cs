using ClinicManagement.DTOs.Requests;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

/// <summary>
/// APIs for managing examination rooms.
/// </summary>
[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly RoomService _roomService;
    public RoomsController(RoomService roomService) => _roomService = roomService;

    /// <summary>
    /// Get all rooms.
    /// </summary>
    /// <response code="200">Returns the list of rooms.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _roomService.FindAllAsync());

    /// <summary>
    /// Get a room by id.
    /// </summary>
    /// <param name="id">Room id.</param>
    /// <response code="200">Returns the room.</response>
    /// <response code="404">Room was not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) => Ok(await _roomService.FindByIdAsync(id));

    /// <summary>
    /// Create a room.
    /// </summary>
    /// <param name="req">Room information.</param>
    /// <response code="201">Room created successfully.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoomRequest req)
    {
        var result = await _roomService.CreateAsync(req);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Update a room.
    /// </summary>
    /// <param name="id">Room id.</param>
    /// <param name="req">New room information.</param>
    /// <response code="200">Room updated successfully.</response>
    /// <response code="404">Room was not found.</response>
    /// <response code="400">Input data is invalid.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] RoomRequest req)
        => Ok(await _roomService.UpdateAsync(id, req));

    /// <summary>
    /// Delete a room.
    /// </summary>
    /// <param name="id">Room id.</param>
    /// <response code="204">Room deleted successfully.</response>
    /// <response code="404">Room was not found.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _roomService.DeleteAsync(id);
        return NoContent();
    }
}

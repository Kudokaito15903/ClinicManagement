using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class RoomService
{
    private readonly ClinicDbContext _db;
    public RoomService(ClinicDbContext db) => _db = db;

    public async Task<List<RoomResponse>> FindAllAsync()
        => await _db.Rooms.Select(r => ToResponse(r)).ToListAsync();

    public async Task<RoomResponse> FindByIdAsync(long id)
        => ToResponse(await GetByIdAsync(id));

    public async Task<RoomResponse> CreateAsync(RoomRequest req)
    {
        var room = new Room
        {
            Code = await GenerateCodeAsync(),
            Name = req.Name,
            Description = req.Description
        };
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
        return ToResponse(room);
    }

    private async Task<string> GenerateCodeAsync()
    {
        long count = await _db.Rooms.LongCountAsync();
        string candidate;
        do
        {
            count++;
            candidate = $"PK{count:D3}";
        } while (await _db.Rooms.AnyAsync(r => r.Code == candidate));
        return candidate;
    }

    public async Task<RoomResponse> UpdateAsync(long id, RoomRequest req)
    {
        var room = await GetByIdAsync(id);
        room.Name = req.Name;
        room.Description = req.Description;
        await _db.SaveChangesAsync();
        return ToResponse(room);
    }

    public async Task DeleteAsync(long id)
    {
        var room = await GetByIdAsync(id);
        _db.Rooms.Remove(room);
        await _db.SaveChangesAsync();
    }

    public async Task<Room> GetByIdAsync(long id)
        => await _db.Rooms.FindAsync(id)
           ?? throw new ResourceNotFoundException($"Phong khong ton tai: {id}");

    public static RoomResponse ToResponse(Room r)
        => new(r.Id, r.Code, r.Name, r.Description);
}

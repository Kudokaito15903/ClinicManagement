using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class SystemConfigService
{
    private readonly ClinicDbContext _db;
    public SystemConfigService(ClinicDbContext db) => _db = db;

    public async Task<List<SystemConfigResponse>> GetAllAsync()
        => await _db.SystemConfigs
            .OrderBy(c => c.ConfigKey)
            .Select(c => ToResponse(c))
            .ToListAsync();

    public async Task<SystemConfigResponse> GetByKeyAsync(string key)
    {
        var config = await _db.SystemConfigs.FindAsync(key)
            ?? throw new ResourceNotFoundException($"Config key không tồn tại: {key}");
        return ToResponse(config);
    }

    public async Task<SystemConfigResponse> UpdateAsync(string key, SystemConfigUpdateRequest req)
    {
        var config = await _db.SystemConfigs.FindAsync(key)
            ?? throw new ResourceNotFoundException($"Config key không tồn tại: {key}");

        config.ConfigValue = req.ConfigValue;
        await _db.SaveChangesAsync();
        return ToResponse(config);
    }

    public static SystemConfigResponse ToResponse(SystemConfig c)
        => new(c.ConfigKey, c.ConfigValue, c.Description);
}

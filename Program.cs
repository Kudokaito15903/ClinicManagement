using ClinicManagement.Data;
using ClinicManagement.Middleware;
using ClinicManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ─── Database ──────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ClinicDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ─── Services ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<DiagnosisService>();
builder.Services.AddScoped<MedicalServiceService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<VisitService>();
builder.Services.AddScoped<VisitServiceService>();
builder.Services.AddScoped<VisitDiagnosisService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<BillingService>();
builder.Services.AddScoped<ReportService>();

// ─── Controllers + JSON ────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// ─── Swagger ───────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clinic Management API",
        Version = "v1",
        Description = "REST API quản lý phòng khám - chuyển đổi từ Spring Boot sang .NET Core"
    });
});

var app = builder.Build();

// ─── Middleware ─────────────────────────────────────────────────────────────
app.UseMiddleware<GlobalExceptionHandler>();

// ─── Swagger UI ─────────────────────────────────────────────────────────────
app.UseSwagger(c => c.RouteTemplate = "api-docs/{documentName}/swagger.json");
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api-docs/v1/swagger.json", "Clinic Management API v1");
    c.RoutePrefix = "swagger-ui.html";
});

app.MapControllers();

// ─── Auto Migrate + Seed ────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
    try
    {
        db.Database.Migrate();
        await DataSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Migration/Seeding failed (database may not be available).");
    }
}

app.Run();

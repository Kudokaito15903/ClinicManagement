using ClinicManagement.Data;
using ClinicManagement.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class ReportService
{
    private readonly ClinicDbContext _db;
    private readonly VisitService _visitService;
    private readonly VisitServiceService _visitServiceService;

    public ReportService(ClinicDbContext db, VisitService visitService,
        VisitServiceService visitServiceService)
    {
        _db = db;
        _visitService = visitService;
        _visitServiceService = visitServiceService;
    }

    public async Task<StatisticsResponse> GetStatisticsAsync()
    {
        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var todayEnd = todayStart.AddDays(1).AddTicks(-1);
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        long totalPatients = await _db.Patients.CountAsync(p => !p.Deleted);
        long visitsToday = await _db.Visits.CountAsync(v => v.VisitDate >= todayStart && v.VisitDate <= todayEnd);
        long visitsThisMonth = await _db.Visits.CountAsync(v => v.VisitDate >= monthStart && v.VisitDate <= todayEnd);

        var revenueToday = await SumRevenueAsync(todayStart, todayEnd);
        var revenueMonth = await SumRevenueAsync(monthStart, todayEnd);
        var revenueTotal = await SumRevenueAsync(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), todayEnd);

        return new StatisticsResponse(totalPatients, visitsToday, visitsThisMonth,
            revenueToday, revenueMonth, revenueTotal);
    }

    public async Task<RevenueReportResponse> GetRevenueReportAsync(DateOnly from, DateOnly to)
    {
        var start = from.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var end = to.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        long totalVisits = await _db.Visits.CountAsync(v => v.VisitDate >= start && v.VisitDate <= end);
        decimal examTotal = await _db.Visits
            .Where(v => v.VisitDate >= start && v.VisitDate <= end)
            .SumAsync(v => (decimal?)v.ExaminationFee) ?? 0;

        decimal svcTotal = await _db.VisitServices
            .Where(vs => vs.Visit.VisitDate >= start && vs.Visit.VisitDate <= end)
            .SumAsync(vs => (decimal?)(vs.UnitPrice * vs.Quantity)) ?? 0;

        // Build daily breakdown
        var visitsByDay = await _db.Visits
            .Where(v => v.VisitDate >= start && v.VisitDate <= end)
            .GroupBy(v => v.VisitDate.Date)
            .Select(g => new { Date = g.Key, Count = g.LongCount(), ExamSum = g.Sum(v => v.ExaminationFee) })
            .ToListAsync();

        var svcByDay = await _db.VisitServices
            .Where(vs => vs.Visit.VisitDate >= start && vs.Visit.VisitDate <= end)
            .GroupBy(vs => vs.Visit.VisitDate.Date)
            .Select(g => new { Date = g.Key, SvcSum = g.Sum(vs => vs.UnitPrice * vs.Quantity) })
            .ToListAsync();

        var svcMap = svcByDay.ToDictionary(x => x.Date, x => x.SvcSum);

        var daily = visitsByDay
            .Select(x =>
            {
                var svc = svcMap.TryGetValue(x.Date, out var s) ? s : 0m;
                return new DailyRevenueResponse(DateOnly.FromDateTime(x.Date), x.Count, x.ExamSum, svc, x.ExamSum + svc);
            })
            .OrderBy(x => x.Date)
            .ToList();

        return new RevenueReportResponse(from.ToString(), to.ToString(),
            totalVisits, examTotal, svcTotal, examTotal + svcTotal, daily);
    }

    public async Task<VisitDetailResponse> GetVisitDetailAsync(long visitId)
    {
        var visit = await _visitService.GetByIdRawAsync(visitId);
        var services = await _visitServiceService.FindByVisitIdAsync(visitId);
        var serviceTotal = services.Sum(s => s.Subtotal);

        return new VisitDetailResponse(
            await _visitService.ToResponseAsync(visit),
            services,
            serviceTotal,
            visit.ExaminationFee,
            serviceTotal + visit.ExaminationFee
        );
    }

    private async Task<decimal> SumRevenueAsync(DateTime from, DateTime to)
    {
        var examFee = await _db.Visits
            .Where(v => v.VisitDate >= from && v.VisitDate <= to)
            .SumAsync(v => (decimal?)v.ExaminationFee) ?? 0;

        var svcFee = await _db.VisitServices
            .Where(vs => vs.Visit.VisitDate >= from && vs.Visit.VisitDate <= to)
            .SumAsync(vs => (decimal?)(vs.UnitPrice * vs.Quantity)) ?? 0;

        return examFee + svcFee;
    }
}

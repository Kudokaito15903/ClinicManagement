using ClinicManagement.Data;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class BillingService
{
    private readonly ClinicDbContext _db;
    private readonly VisitService _visitService;
    private readonly VisitServiceService _visitServiceService;

    public BillingService(ClinicDbContext db, VisitService visitService,
        VisitServiceService visitServiceService)
    {
        _db = db;
        _visitService = visitService;
        _visitServiceService = visitServiceService;
    }

    public async Task<BillResponse> GetBillAsync(long visitId)
    {
        var visit = await _visitService.GetByIdRawAsync(visitId);
        var patient = visit.Patient;
        var payment = visit.Payment;
        var primaryDiagnosis = visit.VisitDiagnoses
            .FirstOrDefault(vd => vd.IsPrimary)?.Diagnosis
            ?? visit.VisitDiagnoses.FirstOrDefault()?.Diagnosis;

        // Đọc tên phòng khám từ system_configs (admin có thể thay đổi qua API)
        var clinicNameConfig = await _db.SystemConfigs.FindAsync("clinic_name");
        var clinicName = clinicNameConfig?.ConfigValue ?? "Phòng khám Đa khoa";

        var vsList = await _db.VisitServices
            .Include(vs => vs.Service)
            .Where(vs => vs.VisitId == visitId)
            .ToListAsync();

        var services = vsList.Select(_visitServiceService.ToResponse).ToList();
        var serviceTotal = services.Sum(s => s.Subtotal);
        var examFee = payment?.ExaminationFee ?? 0;

        return new BillResponse(
            visit.Id,
            visit.VisitDate,
            clinicName,
            patient?.Code ?? "",
            patient?.FullName ?? "",
            patient?.DateOfBirth.Year,
            patient?.Gender.ToString(),
            patient?.Address,
            visit.Doctor?.FullName,
            visit.Room?.Name,
            primaryDiagnosis?.Name,
            examFee,
            visit.Reason,
            services,
            serviceTotal,
            examFee + serviceTotal
        );
    }

    public async Task<string> GetReceiptHtmlAsync(long visitId)
    {
        var bill = await GetBillAsync(visitId);
        var fmt = "dd/MM/yyyy HH:mm";

        var rows = new System.Text.StringBuilder();
        foreach (var svc in bill.Services)
        {
            rows.Append($"<tr><td>{svc.ServiceName}</td>");
            rows.Append($"<td class=\"right\">{svc.Quantity:N0}</td>");
            rows.Append($"<td class=\"right\">{svc.UnitPrice:N0}</td>");
            rows.Append($"<td class=\"right\">{svc.Subtotal:N0}</td></tr>");
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"vi\"><head><meta charset=\"UTF-8\">");
        sb.AppendLine($"<title>Phieu thu - {bill.PatientName}</title>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial, sans-serif; font-size: 14px; margin: 40px; color: #333; }");
        sb.AppendLine("h1 { text-align: center; font-size: 20px; margin-bottom: 4px; }");
        sb.AppendLine(".clinic { text-align: center; font-size: 13px; color: #666; margin-bottom: 20px; }");
        sb.AppendLine(".receipt-title { text-align: center; font-size: 18px; font-weight: bold; text-transform: uppercase; margin: 16px 0; letter-spacing: 2px; }");
        sb.AppendLine("table.info { width: 100%; border-collapse: collapse; margin-bottom: 16px; }");
        sb.AppendLine("table.info td { padding: 4px 8px; }");
        sb.AppendLine("table.info td:first-child { width: 180px; font-weight: bold; }");
        sb.AppendLine("table.services { width: 100%; border-collapse: collapse; margin-bottom: 16px; }");
        sb.AppendLine("table.services th, table.services td { border: 1px solid #ccc; padding: 8px; }");
        sb.AppendLine("table.services th { background: #f0f0f0; text-align: center; }");
        sb.AppendLine(".right { text-align: right; }");
        sb.AppendLine(".total-row { font-weight: bold; background: #fff8e1; }");
        sb.AppendLine(".grand-total { font-size: 16px; font-weight: bold; text-align: right; margin-top: 12px; }");
        sb.AppendLine(".footer { text-align: right; margin-top: 40px; }");
        sb.AppendLine("hr { border: none; border-top: 1px solid #ddd; margin: 12px 0; }");
        sb.AppendLine("</style></head><body>");
        sb.AppendLine($"<h1>{bill.ClinicName}</h1>");
        sb.AppendLine("<div class=\"clinic\">Phieu kham benh &amp; thanh toan</div>");
        sb.AppendLine("<div class=\"receipt-title\">Phieu Thu Tien</div><hr>");
        sb.AppendLine("<table class=\"info\">");
        sb.AppendLine($"<tr><td>Ngay kham:</td><td>{bill.VisitDate?.ToString(fmt)}</td></tr>");
        sb.AppendLine($"<tr><td>Ma benh nhan:</td><td>{bill.PatientCode}</td></tr>");
        sb.AppendLine($"<tr><td>Ho ten:</td><td>{bill.PatientName}</td></tr>");
        sb.AppendLine($"<tr><td>Nam sinh:</td><td>{bill.PatientBirthYear}</td></tr>");
        sb.AppendLine($"<tr><td>Gioi tinh:</td><td>{bill.PatientGender}</td></tr>");
        sb.AppendLine($"<tr><td>Dia chi:</td><td>{bill.PatientAddress}</td></tr>");
        sb.AppendLine($"<tr><td>Bac si:</td><td>{bill.DoctorName}</td></tr>");
        sb.AppendLine($"<tr><td>Phong kham:</td><td>{bill.RoomName}</td></tr>");
        sb.AppendLine($"<tr><td>Chan doan:</td><td>{bill.DiagnosisName}</td></tr>");
        sb.AppendLine("</table><hr>");
        sb.AppendLine("<table class=\"services\"><thead><tr>");
        sb.AppendLine("<th>Ten dich vu</th><th>SL</th><th>Don gia (VND)</th><th>Thanh tien (VND)</th>");
        sb.AppendLine("</tr></thead><tbody>");
        sb.AppendLine(rows.ToString());
        sb.AppendLine($"<tr class=\"total-row\"><td colspan=\"3\" class=\"right\">Tong tien dich vu:</td><td class=\"right\">{bill.ServiceTotal:N0}</td></tr>");
        sb.AppendLine($"<tr class=\"total-row\"><td colspan=\"3\" class=\"right\">Tien kham bac si:</td><td class=\"right\">{bill.ExaminationFee:N0}</td></tr>");
        sb.AppendLine("</tbody></table>");
        sb.AppendLine($"<div class=\"grand-total\">TONG CONG: {bill.GrandTotal:N0} VND</div>");
        sb.AppendLine("<div class=\"footer\"><p>Thu ngan ky ten</p><br><br><p>____________________</p></div>");
        sb.AppendLine("</body></html>");
        return sb.ToString();
    }
}

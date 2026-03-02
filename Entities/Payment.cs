namespace ClinicManagement.Entities;

public enum PaymentMethod { Cash, Card, Transfer }

public class Payment
{
    public long Id { get; set; }

    public long VisitId { get; set; }                           // 1 Visit = 1 Payment
    public Visit Visit { get; set; } = null!;

    public long? CashierId { get; set; }                        // Nhân viên thu ngân
    public User? Cashier { get; set; }

    public decimal ExaminationFee { get; set; }                 // Phí khám cố định
    public decimal ServiceTotal { get; set; }                   // Tổng tiền dịch vụ (snapshot)
    public decimal GrandTotal { get; set; }                     // Tổng trước giảm giá
    public decimal Discount { get; set; } = 0;
    public decimal FinalAmount { get; set; }                    // Số tiền thực thu

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    public DateTime? PaidAt { get; set; }
    public string? CashierNote { get; set; }
    public DateTime CreatedAt { get; set; }
}

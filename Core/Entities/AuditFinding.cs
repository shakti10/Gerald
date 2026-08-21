
namespace Gerald.Core.Entities
{
    public class AuditFinding
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string Severity { get; set; } // Low, Medium, High
        public string Status { get; set; } // Open, Closed, In Progress
        public string Notes { get; set; }
        public DateTime FindingDate { get; set; }
        public Vendor Vendor { get; set; }
    }
}
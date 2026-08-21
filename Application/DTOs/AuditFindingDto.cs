namespace Gerald.Application.DTOs
{
    public class AuditFindingDto
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public DateTime FindingDate { get; set; }
    }
}
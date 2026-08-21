namespace Gerald.Core.Entities
{
   public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; } // Active, Inactive, Under Review
        public ICollection<AuditFinding> Findings { get; set; } = new List<AuditFinding>();
    }
}

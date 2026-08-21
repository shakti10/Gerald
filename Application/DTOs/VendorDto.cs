namespace Gerald.Application.DTOs
{
    public class VendorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public List<AuditFindingDto> Findings { get; set; }
    }
}
namespace Gerald.Application.DTOs
{
    public class RiskAssessmentDto
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string RiskRating { get; set; } // Low, Medium, High
        public string Summary { get; set; }
        public List<string> KeyFindings { get; set; }
        public string Reasoning { get; set; }
    }
}
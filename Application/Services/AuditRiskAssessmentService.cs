
namespace Gerald.Application.Services
{
    public class AuditRiskAssessmentService
    {
        public RiskAssessmentDto AssessVendorRisk(Vendor vendor)
        {
            if (vendor?.Findings == null || vendor.Findings.Count == 0)
            {
                return new RiskAssessmentDto
                {
                    VendorId = vendor.Id,
                    VendorName = vendor.Name,
                    RiskRating = "Low",
                    Summary = "No findings recorded.",
                    KeyFindings = new List<string>(),
                    Reasoning = "Vendor has no audit findings. Default to Low risk."
                };
            }

            var highSeverityCount = vendor.Findings.Count(f => f.Severity == "High");
            var mediumSeverityCount = vendor.Findings.Count(f => f.Severity == "Medium");
            var lowSeverityCount = vendor.Findings.Count(f => f.Severity == "Low");
            var openCount = vendor.Findings.Count(f => f.Status == "Open");

            // Deterministic risk scoring
            string riskRating = DetermineRiskRating(highSeverityCount, mediumSeverityCount, openCount);

            var keyFindings = vendor.Findings
                .OrderByDescending(f => f.Severity == "High" ? 3 : f.Severity == "Medium" ? 2 : 1)
                .Take(5)
                .Select(f => $"{f.Severity}: {f.Notes}")
                .ToList();

            var summary = $"Vendor {vendor.Name} has {vendor.Findings.Count} total findings: " +
                         $"{highSeverityCount} High, {mediumSeverityCount} Medium, {lowSeverityCount} Low. " +
                         $"{openCount} findings remain open.";

            var reasoning = GenerateReasoning(highSeverityCount, mediumSeverityCount, openCount);

            return new RiskAssessmentDto
            {
                VendorId = vendor.Id,
                VendorName = vendor.Name,
                RiskRating = riskRating,
                Summary = summary,
                KeyFindings = keyFindings,
                Reasoning = reasoning
            };
        }

        private string DetermineRiskRating(int highCount, int mediumCount, int openCount)
        {
            if (highCount >= 2 || (highCount > 0 && openCount > 2))
                return "High";
            if (highCount == 1 || mediumCount >= 3 || openCount >= 4)
                return "Medium";
            return "Low";
        }

        private string GenerateReasoning(int highCount, int mediumCount, int openCount)
        {
            var reasons = new List<string>();

            if (highCount >= 2)
                reasons.Add($"Multiple high-severity findings ({highCount}) detected");
            if (highCount == 1)
                reasons.Add("One high-severity finding present");
            if (mediumCount >= 3)
                reasons.Add($"Elevated medium-severity findings ({mediumCount})");
            if (openCount >= 4)
                reasons.Add($"Too many unresolved issues ({openCount} open)");
            if (reasons.Count == 0)
                reasons.Add("Low volume of findings and mostly resolved issues");

            return string.Join("; ", reasons) + ".";
        }
    }
}
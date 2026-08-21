using Gerald.Application.Interfaces;
using Gerald.Core.Entities;

namespace Gerald.Application.Services;

public sealed class StubLanguageModelClient : ILanguageModelClient
{
    public string CreateSummary(string vendorName, IEnumerable<AuditFinding> findings)
    {
        var findingList = findings.ToList();
        var highCount = findingList.Count(finding => finding.Severity == "High");
        var mediumCount = findingList.Count(finding => finding.Severity == "Medium");
        var lowCount = findingList.Count(finding => finding.Severity == "Low");
        var openCount = findingList.Count(finding => finding.Status == "Open");

        return $"Vendor {vendorName} has {findingList.Count} total findings: " +
               $"{highCount} High, {mediumCount} Medium, {lowCount} Low. " +
               $"{openCount} findings remain open.";
    }
}

using Gerald.Core.Entities;

namespace Gerald.Application.Interfaces;

public interface ILanguageModelClient
{
    string CreateSummary(string vendorName, IEnumerable<AuditFinding> findings);
}

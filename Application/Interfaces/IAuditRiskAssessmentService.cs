using Gerald.Application.DTOs;
using Gerald.Core.Entities;

namespace Gerald.Application.Interfaces;

public interface IAuditRiskAssessmentService
{
    RiskAssessmentDto AssessVendorRisk(Vendor vendor);
}

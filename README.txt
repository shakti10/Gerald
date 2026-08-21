# Vendor Audit Management - AI Risk Assessment Tool

## Summary
A minimal, production-ready ASP.NET Core 10 Web API for vendor audit management with AI-assisted risk assessment. Auditors track vendor findings; the system summarizes findings and proposes risk ratings (Low/Medium/High) with explainable reasoning.

## What's Built
- **Vendor Management API**: List vendors, view details with audit findings
- **Audit Finding Tracking**: Severity (Low/Medium/High), status (Open/Closed), notes, dates
- **AI Risk Assessment**: Deterministic risk scoring based on finding patterns
- **Sample Data**: 5 vendors with 12 audit findings seeded on startup
- **Angular-Ready CORS**: Configured for http://localhost:4200

## Quick Start

### Prerequisites
- .NET SDK 10.0.300

### 1. Clone & Setup
```powershell
git clone https://github.com/shakti10/Gerald.git
cd Gerald
dotnet restore
```

### 2. Run
```powershell
dotnet run
```
API runs on `http://localhost:5085` | Swagger UI: `http://localhost:5085/swagger`

### 3. Database
- SQLite database (`audit.db`) is auto-created on first run
- Sample data (5 vendors + 12 findings) is auto-seeded

## API Endpoints

### Vendors
- `GET /api/vendors` - List all vendors with findings
- `GET /api/vendors/{id}` - Get vendor detail with findings
- `POST /api/vendors/{id}/risk-assessment` - Get AI risk summary & rating

### Example: Get Risk Assessment
```powershell
$response = Invoke-RestMethod -Uri "http://localhost:5085/api/vendors/1/risk-assessment" -Method Post
$response | ConvertTo-Json
```

Response:
```json
{
  "vendorId": 1,
  "vendorName": "Acme Corp",
  "riskRating": "High",
  "summary": "Vendor Acme Corp has 3 total findings: 1 High, 1 Medium, 1 Low. 1 findings remain open.",
  "keyFindings": [
	"High: Missing compliance certification",
	"Medium: Documentation outdated",
	"Low: Minor process deviation"
  ],
  "reasoning": "Multiple high-severity findings (1) detected; One high-severity finding present; Elevated medium-severity findings (1)."
}
```

## Architecture

**Layers** (Clean Architecture):
- **Presentation** (`Controllers/`): HTTP endpoints
- **Application** (`Services/`, `DTOs/`): Business logic & data transfer
- **Core** (`Entities/`): Domain models
- **Infrastructure** (`Repositories/`, `Data/`): Data access & EF Core

**Key Components**:
- `GeraldDbContext`: Single EF Core DbContext (Vendor, AuditFinding entities)
- `VendorRepository`: Data access for vendors & findings
- `AuditRiskAssessmentService`: Deterministic risk scoring algorithm

## Risk Scoring Logic

Deterministic algorithm based on finding patterns:
- **High Risk**: 2+ high-severity findings OR (1 high + 3+ open findings)
- **Medium Risk**: 1 high-severity OR 3+ medium-severity OR 4+ open findings
- **Low Risk**: Default; minimal or resolved findings

Reasoning is explicit and grounded in the data.

## Design Decisions & Trade-offs

1. **No Auth (MVP)**: Kept auth minimal for 2-hour scope; easy to add if needed
2. **Deterministic Risk Scoring**: No LLM calls; explainable, testable rules
3. **SQLite**: Fast setup; easy to switch to SQL Server for production
4. **Repository Pattern**: Simple, single-purpose; no generic base needed
5. **DTO-based API**: Decouples API contract from domain entities
6. **Seed Data Only**: No real company/customer data; sample vendors for demo

## What I Verified
- Build compiles cleanly (.NET 10)
- API endpoints respond (tested via Swagger)
- Seed data loads on startup
- Risk assessment logic produces consistent ratings
- CORS configured for Angular (port 4200)

## What Would Be Improved With More Time

1. **Authentication**: JWT tokens + role-based access (Auditor, Reviewer, Admin)
2. **Real LLM Integration**: Swap deterministic scoring for Azure OpenAI summarization
3. **Auditability**: Audit logs for all changes (who, what, when)
4. **Data Protection**: Encryption at rest for sensitive findings
5. **Pagination & Filtering**: For large datasets
6. **Unit & Integration Tests**: Xunit test suite for services & endpoints
7. **Angular Frontend**: Vendor list, detail view, risk summary display
8. **Error Handling Middleware**: Centralized exception handling
9. **API Versioning**: v1, v2 support if schema changes
10. **Production Config**: Separate appsettings for Dev/Staging/Prod

## Security Notes
- **Current State**: No authentication (suitable for demo/evaluation)
- **For Production**: Add JWT auth, enforce HTTPS, rate limiting, CORS restrictions
- **Audit Data**: Implement audit trails, encrypt sensitive findings, restrict access by role
- **Data Protection**: Implement row-level security (RLS) for multi-tenant scenarios

## Tech Stack
- Runtime: .NET 10
- Framework: ASP.NET Core 10
- Database: Entity Framework Core + SQLite
- API Documentation: Swagger (OpenAPI)
- Architecture: Clean Architecture, Repository Pattern

## Troubleshooting

**Port Already in Use:**
Edit `Properties/launchSettings.json`, change port to 5086:
```json
"applicationUrl": "http://localhost:5086"
```

**Database Lock Errors:**
```powershell
Remove-Item -Path audit.db* -Force
dotnet run  # Creates fresh database
```

**CORS Issues:**
Verify Angular runs on `http://localhost:4200` or update CORS policy in Program.cs.

## Next Steps
1. Build Angular UI (vendor list, detail, risk summary views)
2. Add authentication & authorization
3. Integrate real LLM for finding summarization
4. Deploy to Azure App Service + SQL Server
5. Add comprehensive test coverage


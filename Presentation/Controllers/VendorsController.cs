using Gerald.Application.Interfaces;

namespace Gerald.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly VendorRepository _vendorRepository;
        private readonly IAuditRiskAssessmentService _riskService;

        public VendorsController(VendorRepository vendorRepository, IAuditRiskAssessmentService riskService)
        {
            _vendorRepository = vendorRepository;
            _riskService = riskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VendorDto>>> GetAllVendors()
        {
            var vendors = await _vendorRepository.GetAllAsync();
            var dtos = vendors.Select(v => new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                RegistrationNumber = v.RegistrationNumber,
                RegistrationDate = v.RegistrationDate,
                Status = v.Status,
                Findings = v.Findings.Select(f => new AuditFindingDto
                {
                    Id = f.Id,
                    VendorId = f.VendorId,
                    Severity = f.Severity,
                    Status = f.Status,
                    Notes = f.Notes,
                    FindingDate = f.FindingDate
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VendorDto>> GetVendorById(int id)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);
            if (vendor == null)
                return NotFound();

            return Ok(new VendorDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                RegistrationNumber = vendor.RegistrationNumber,
                RegistrationDate = vendor.RegistrationDate,
                Status = vendor.Status,
                Findings = vendor.Findings.Select(f => new AuditFindingDto
                {
                    Id = f.Id,
                    VendorId = f.VendorId,
                    Severity = f.Severity,
                    Status = f.Status,
                    Notes = f.Notes,
                    FindingDate = f.FindingDate
                }).ToList()
            });
        }

        [HttpPost("{id}/risk-assessment")]
        public async Task<ActionResult<RiskAssessmentDto>> GetRiskAssessment(int id)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);
            if (vendor == null)
                return NotFound();

            var assessment = _riskService.AssessVendorRisk(vendor);
            return Ok(assessment);
        }
    }
}